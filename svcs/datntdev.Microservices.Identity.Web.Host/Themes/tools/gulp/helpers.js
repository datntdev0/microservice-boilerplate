import gulp from "gulp";
import sass from "gulp-dart-sass";
import rename from "gulp-rename";
import rewrite from "gulp-rewrite-css";
import concat from "gulp-concat";
import lazypipe from "lazypipe";
import gulpif from "gulp-if";
import terser from "gulp-terser";
import sourcemaps from "gulp-sourcemaps";
import path, * as pathDir from "path";
import fs from "fs";
import cleancss from "gulp-clean-css";
import { glob } from "glob";
import { fileURLToPath } from 'url';
import { gulpConfig } from "../gulp.config.js";

// merge with default parameters
const args = Object.assign(
    {
        prod: false,
        sass: false,
        js: false,
        media: false,
    }
);

let allAssets = false;
if (args.sass === false && args.js === false && args.media === false) {
    allAssets = true;
}

// default variable config
const config = Object.assign(
    {},
    {
        demo: "",
        debug: true,
        compile: {
            jsMinify: false,
            cssMinify: false,
            jsSourcemaps: false,
            cssSourcemaps: false,
        },
    },
    gulpConfig.config
);

/**
 * Walk into object recursively
 * @param array
 * @param funcname
 * @param userdata
 * @returns {boolean}
 */
function objectWalkRecursive(array, funcname, userdata) {
    if (!array || typeof array !== 'object') {
        return false;
    }
    if (typeof funcname !== 'function') {
        return false;
    }

    for (let key in array) {
        // apply "funcname" recursively only on object
        if (Object.prototype.toString.call(array[key]) === '[object Object]') {
            const funcArgs = [array[key], funcname];
            if (arguments.length > 2) {
                funcArgs.push(userdata);
            }
            if (objectWalkRecursive.apply(null, funcArgs) === false) {
                return false;
            }
            // continue
        }
        try {
            if (arguments.length > 2) {
                funcname(array[key], key, userdata);
            }
            else {
                funcname(array[key], key);
            }
        }
        catch (e) {
            console.error('e', e);
            return false;
        }
    }
    return true;
};

/**
 * Add JS compilation options to gulp pipe
 */
const jsChannel = () => {
    const { compile } = config;
    const { jsSourcemaps, jsMinify } = compile;
    return lazypipe()
        .pipe(() => {
            return gulpif(
                jsSourcemaps,
                sourcemaps.init({ loadMaps: true, debug: config.debug }),
                sourcemaps.init({ loadMaps: true, debug: config.debug }),
            );
        })
        .pipe(() => {
            return gulpif(jsMinify, terser());
        })
        .pipe(() => {
            return gulpif(
                jsSourcemaps,
                sourcemaps.write("./"),
                sourcemaps.write(null, { addComment: false })
            );
        });
};

/**
 * Add CSS compilation options to gulp pipe
 */
const cssChannel = () => {
    const { compile } = config;
    const { cssSourcemaps, cssMinify } = compile;
    return lazypipe()
        .pipe(() => {
            return gulpif(
                cssSourcemaps,
                sourcemaps.init({ loadMaps: true, debug: config.debug })
            );
        })
        .pipe(() => {
            return sass({
                errLogToConsole: true,
                includePaths: [gulpConfig.config.path.src + "/sass", "node_modules"],
            }).on("error", sass.logError);
        })
        .pipe(() => {
            return gulpif(cssMinify, cleancss());
        })
        .pipe(() => {
            return gulpif(cssSourcemaps, sourcemaps.write("./"));
        });
};

/**
 * Multiple output paths by output config
 * @param path
 * @param outputFile
 * @param type
 * @returns {*}
 */
const outputChannel = (path, outputFile, type) => {
    if (!allAssets) {
        if (args.sass && ["styles"].indexOf(type) === -1) {
            return lazypipe().pipe(() => {
                // noop
            });
        }
        if (args.js && ["scripts"].indexOf(type) === -1) {
            return lazypipe().pipe(() => {
                // noop
            });
        }
        if (args.media && ["media", "fonts", "images"].indexOf(type) === -1) {
            return lazypipe().pipe(() => {
                // noop
            });
        }
    }

    if (typeof path === "undefined") {
        console.log("Output path not defined");
    }
    if (typeof outputFile === "undefined") {
        outputFile = "";
    }

    let piping = lazypipe();

    if (type === "styles") {
        piping = piping.pipe(() => {
            return gulpif(
                gulpConfig.config.compile.cssMinify,
                rename({ suffix: args.suffix ? ".min" : "" })
            );
        });
    }

    if (type === "scripts") {
        piping = piping.pipe(() => {
            return gulpif(
                gulpConfig.config.compile.jsMinify,
                rename({ suffix: args.suffix ? ".min" : "" })
            );
        });
    }

    const regex = new RegExp(/\{\$.*?\}/);
    const matched = path.match(regex);
    // {$config.dist}/plugins/global/fonts/fonticon
    if (matched) {
        const outputs = gulpConfig.config.dist;
        outputs.forEach((output) => {
            let outputPath = path.replace(matched[0], output).replace(outputFile, "");
            (function (_output) {
                piping = piping.pipe(() => {
                    return gulp.dest(_output);
                });
            })(outputPath);
        });
    }

    if (typeof cb !== 'undefined') {
        cb();
    }

    return piping;
};

/**
 * Convert string path to actual path
 * @param path
 * @returns {*}
 */
const dotPath = (path) => {
    const regex = new RegExp(/\{\$(.*?)\}/),
        dot = (obj, i) => {
            return obj[i];
        };
    const matched = path.match(regex);
    if (matched) {
        const realpath = matched[1].split(".").reduce(dot, gulpConfig);
        return path.replace(matched[0], realpath);
    }

    return path;
};

/**
 * Convert multiple paths
 * @param paths
 */
const dotPaths = (paths) => {
    paths.forEach((path, i) => {
        paths[i] = dotPath(path);
    });
};

/**
 * Css path rewriter when bundle files moved
 * @param bundle
 */
const cssRewriter = (bundle) => {
    const imgRegex = new RegExp(/\.(gif|jpg|jpeg|tiff|png|ico|svg)$/i);

    return lazypipe().pipe(() => {
        // rewrite css relative path
        return rewrite({
            destination: bundle["styles"],
            debug: config.debug,
            adaptPath: (ctx) => {
                const isCss = ctx.sourceFile.match(/\.[css]+$/i);
                // process css only
                if (isCss[0] === ".css") {
                    if (/plugins.*?bundle/.test(bundle["styles"])) {
                        const pieces = ctx.sourceDir.split(/\\|\//);
                        // only vendors/base pass this
                        let vendor = pieces[pieces.indexOf("node_modules") + 1];
                        if (pieces.indexOf("node_modules") === -1) {
                            vendor = pieces[pieces.indexOf("plugins") + 1];
                        }

                        let extension = "fonts/";
                        if (imgRegex.test(ctx.targetFile)) {
                            extension = "images/";
                        }

                        return path.join(extension, vendor, path.basename(ctx.targetFile));
                    }

                    return ctx.targetFile.replace(/\.?\.\//, "");
                }
            },
        });
    });
};

/**
 * Get end filename from path
 * @param path
 * @returns {string}
 */
const baseFileName = (path) => {
    const maybeFile = path.split("/").pop();
    if (maybeFile.indexOf(".") !== -1) {
        return maybeFile;
    }
    return "";
};

const baseName = (str, extension) => {
    let base = new String(str).substring(str.lastIndexOf("/") + 1);
    if (!extension && base.lastIndexOf(".") != -1) {
        base = base.substring(0, base.lastIndexOf("."));
    }
    return base;
};

/**
 * Remove file name and get the path
 */
const pathOnly = (str) => {
    const array = str.split("/");
    if (array.length > 0) {
        array.pop();
    }

    return array.join("/");
};

const getFolders = (dir) => {
    try {
        return fs.readdirSync(dir, { withFileTypes: true })
            .filter((dirent) => dirent.isDirectory())
            .map((dirent) => dirent.name);
    } catch (e) {
        return [];
    }
};

const bundleStreams = [];

/**
 * Bundle
 * @param bundle
 */
const bundle = (bundle) => {
    let stream;

    if (bundle.hasOwnProperty("src") && bundle.hasOwnProperty("dist")) {
        // for images & fonts as per vendor
        if ("mandatory" in bundle.src && "optional" in bundle.src) {
            let vendors = {};

            for (let key in bundle.src) {
                if (!bundle.src.hasOwnProperty(key)) {
                    continue;
                }
                if (key === "override") {
                    continue;
                }
                vendors = Object.assign(vendors, bundle.src[key]);
            }

            for (let vendor in vendors) {
                if (!vendors.hasOwnProperty(vendor)) {
                    continue;
                }

                const vendorObj = vendors[vendor];

                for (let type in vendorObj) {
                    if (!vendorObj.hasOwnProperty(type)) {
                        continue;
                    }

                    dotPaths(vendorObj[type]);

                    switch (type) {
                        case "fonts":
                            stream = gulp.src(vendorObj[type], { allowEmpty: true });
                            const outputFonts = outputChannel(bundle.dist[type] + "/" + vendor, undefined, type)();
                            if (outputFonts) {
                                stream.pipe(outputFonts);
                            }
                            bundleStreams.push(stream);
                            break;
                        case "images":
                            stream = gulp.src(vendorObj[type], { allowEmpty: true });
                            const outputImages = outputChannel(bundle.dist[type] + "/" + vendor, undefined, type)();
                            if (outputImages) {
                                stream.pipe(outputImages);
                            }
                            bundleStreams.push(stream);
                            break;
                    }
                }
            }
        }

        // flattening array
        if (!("styles" in bundle.src) && !("scripts" in bundle.src)) {
            const src = { styles: [], scripts: [] };
            objectWalkRecursive(bundle.src, (paths, type) => {
                switch (type) {
                    case "styles":
                    case "scripts":
                        src[type] = src[type].concat(paths);
                        break;
                    case "images":
                        // images for mandatory and optional vendor already processed
                        if (!"mandatory" in bundle.src || !"optional" in bundle.src) {
                            src[type] = src[type].concat(paths);
                        }
                        break;
                }
            });
            bundle.src = src;
        }

        for (let type in bundle.src) {
            if (!bundle.src.hasOwnProperty(type)) {
                continue;
            }
            // skip if not array
            if (
                Object.prototype.toString.call(bundle.src[type]) !== "[object Array]"
            ) {
                continue;
            }
            // skip if no bundle output is provided
            if (typeof bundle.dist[type] === "undefined") {
                continue;
            }

            dotPaths(bundle.src[type]);
            const outputFile = baseFileName(bundle.dist[type]);

            switch (type) {
                case "styles":
                    if (bundle.dist.hasOwnProperty(type)) {
                        // default css bundle
                        stream = gulp
                            .src(bundle.src[type], { allowEmpty: true })
                            .pipe(cssRewriter(bundle.dist)())
                            // .pipe(concat(outputFile))
                            .pipe(cssChannel()());
                        const outputDefaultCSSBundle = outputChannel(bundle.dist[type], outputFile, type)();
                        if (outputDefaultCSSBundle) {
                            stream.pipe(outputDefaultCSSBundle);
                        }
                        bundleStreams.push(stream);
                    }
                    break;

                case "scripts":
                    if (bundle.dist.hasOwnProperty(type)) {
                        stream = gulp
                            .src(bundle.src[type], { allowEmpty: true })
                            .pipe(concat(outputFile))
                            .pipe(jsChannel()())
                            .on("error", console.error.bind(console));
                        const outputScripts = outputChannel(bundle.dist[type], outputFile, type)();
                        if (outputScripts) {
                            stream.pipe(outputScripts);
                        }
                        bundleStreams.push(stream);
                    }

                    break;

                case "media":
                case "fonts":
                case "images":
                    if (bundle.dist.hasOwnProperty(type)) {
                        stream = gulp.src(bundle.src[type], { allowEmpty: true });
                        const outputImages = outputChannel(bundle.dist[type], undefined, type)();
                        if (outputImages) {
                            stream.pipe(outputImages);
                        }
                        bundleStreams.push(stream);
                    }
                    break;

                case "folder":
                    if (bundle.dist.hasOwnProperty(type)) {
                        // Process folder copying - pattern like {$config.path.node_modules}/tinymce/{skins,models}
                        bundle.src[type].forEach(folderPath => {
                            // Extract base directory path (everything before the last slash)
                            const basePath = folderPath.substring(0, folderPath.lastIndexOf('/'));

                            // Use ** glob pattern to get all files and preserve folder structure
                            const folderGlob = folderPath + '/**/*';

                            stream = gulp.src(dotPath(folderGlob), {
                                allowEmpty: true,
                                base: dotPath(basePath) // Set the base directory to preserve structure
                            });

                            // Handle the output path
                            const outputPath = bundle.dist[type];
                            const output = outputChannel(outputPath, undefined, "media")();
                            if (output) {
                                stream.pipe(output);
                            }
                            bundleStreams.push(stream);
                        });
                    }
                    break;
            }
        }
    }

    return bundleStreams;
};

const outputFuncStreams = [];

/**
 * Copy source to output destination
 * @param bundle
 */
const outputFunc = (bundle) => {
    let stream;

    if (bundle.hasOwnProperty("src") && bundle.hasOwnProperty("dist")) {
        for (let type in bundle.src) {
            if (!bundle.src.hasOwnProperty(type)) {
                continue;
            }

            dotPaths(bundle.src[type]);

            if (bundle.dist.hasOwnProperty(type)) {
                switch (type) {
                    case "styles":
                        stream = gulp
                            .src(bundle.src[type], { allowEmpty: true })
                            .pipe(cssChannel()());
                        const outputStyles = outputChannel(bundle.dist[type], undefined, type)();
                        if (outputStyles) {
                            stream.pipe(outputStyles);
                        }
                        outputFuncStreams.push(stream);
                        break;
                    case "scripts":
                        /**
                         * START: bundle by folder
                         */
                        bundle.src[type].forEach((file) => {
                            const needBundleFileWildLocation = file.replace(
                                "*.js",
                                "bundle/*.js"
                            );

                            const __filename = fileURLToPath(import.meta.url);
                            const __dirname = pathDir.dirname(__filename);
                            const needBundleFiles = glob.sync(
                                path.resolve(__dirname, "../" + needBundleFileWildLocation)
                            );
                            if (needBundleFiles.length > 0) {
                                const needBundleByGroup = [];
                                needBundleFiles.forEach((js) => {
                                    const p = path.dirname(js);
                                    if (typeof needBundleByGroup[p] === "undefined") {
                                        needBundleByGroup[p] = [];
                                    }
                                    needBundleByGroup[p].push(js);
                                });

                                for (let g in needBundleByGroup) {
                                    if (!needBundleByGroup.hasOwnProperty(g)) {
                                        continue;
                                    }

                                    const files = needBundleByGroup[g];

                                    // remove ${config.dist}
                                    const separatorDir = bundle.dist[type].replace(
                                        /{\$config\.dist}/,
                                        ""
                                    );
                                    // get sub dir
                                    const needBundleDir = files[0].split(separatorDir)[1];
                                    const needBundleOutputPath = needBundleDir.replace(
                                        /^(.*?)\/(bundle)\/.*?\.(js)$/,
                                        "$1.$2.$3"
                                    );

                                    const needBundleStream = gulp
                                        .src(files)
                                        .pipe(concat(needBundleOutputPath))
                                        .pipe(jsChannel()())
                                        .on("error", console.error.bind(console));
                                    const needBundleOutput = outputChannel(bundle.dist[type], undefined, type)();
                                    if (needBundleOutput) {
                                        needBundleStream.pipe(needBundleOutput);
                                    }
                                    outputFuncStreams.push(needBundleStream);

                                    // exclude bundle folder from next gulp process
                                    bundle.src[type].push(
                                        "!" +
                                        path
                                            .dirname(needBundleFileWildLocation)
                                            .replace(/\*+?\/bundle/, "") +
                                        needBundleDir.replace(/\/bundle.*?$/, "/**")
                                    );
                                }
                            }
                        });
                        /**
                         * END: bundle by folder
                         */

                        stream = gulp
                            .src(bundle.src[type], { allowEmpty: true })
                            .pipe(jsChannel()())
                            .on("error", console.error.bind(console));
                        const output2 = outputChannel(bundle.dist[type], undefined, type)();
                        if (output2) {
                            stream.pipe(output2);
                        }
                        outputFuncStreams.push(stream);
                        break;
                    case "folder":
                        // Process folder copying - pattern like {$config.path.node_modules}/tinymce/{skins,models}
                        bundle.src[type].forEach(folderPath => {
                            // Extract base directory path (everything before the last slash)
                            const basePath = folderPath.substring(0, folderPath.lastIndexOf('/'));

                            // Use ** glob pattern to get all files and preserve folder structure
                            const folderGlob = folderPath + '/**/*';

                            stream = gulp.src(dotPath(folderGlob), {
                                allowEmpty: true,
                                base: dotPath(basePath) // Set the base directory to preserve structure
                            });

                            // Handle the output path
                            const outputPath = bundle.dist[type];
                            const output = outputChannel(outputPath, undefined, "media")();
                            if (output) {
                                stream.pipe(output);
                            }
                            outputFuncStreams.push(stream);
                        });
                        break;
                    default:
                        stream = gulp.src(bundle.src[type], { allowEmpty: true });
                        const outputDefault = outputChannel(bundle.dist[type], undefined, type)();
                        if (outputDefault) {
                            stream.pipe(outputDefault);
                        }
                        outputFuncStreams.push(stream);
                        break;
                }
            }
        }
    }

    return outputFuncStreams;
};

// Exports
export {
    objectWalkRecursive,
    dotPath,
    pathOnly,
    outputFunc,
    bundle,
    getFolders,
};
