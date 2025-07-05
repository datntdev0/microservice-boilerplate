/**
 * * @fileoverview This file contains the JavaScript code for the Identity Web Host theme.
 */

function signout() {
    fetch('/api/me/signout', { method: 'POST' }).then(() => location.reload())
}