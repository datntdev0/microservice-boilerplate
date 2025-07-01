document.addEventListener('DOMContentLoaded', () => {
    // Get the saved theme from localStorage
    let savedTheme = localStorage.getItem('theme') ?? 'light';

    // Apply the theme to the HTML root element
    document.documentElement.setAttribute('data-bs-theme', savedTheme);

    // Save theme to localStorage
    localStorage.setItem('theme', savedTheme);
});