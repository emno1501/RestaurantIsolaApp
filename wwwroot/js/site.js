// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Mobilmeny publika webbplatsen
let navigation = document.getElementById("navigation");
let hamburger = document.getElementById("hamburger")
if (hamburger != null) {
    hamburger.addEventListener("click", mobileMenu, false);
}

function mobileMenu() {
    if (navigation.className == "restaurantNav defaultMenu") {
        navigation.className = "restaurantNav openMenu";
        hamburger.src = "../images/close.svg"
    } else {
        navigation.className = "restaurantNav defaultMenu";
        hamburger.src = "../images/hamburger.svg"
    }
}

// Undermeny admin-del
let adminNav = document.getElementById("leftNavigation");
let updownBtn = document.getElementById("adminNavDown");
if (updownBtn != null) {
    updownBtn.addEventListener("click", adminNavDown, false);
}

function adminNavDown() {
    if (adminNav.className == "adminLeftNav leftNavDefault") {
        adminNav.className = "adminLeftNav leftNavDown";
        updownBtn.src = "../images/up_black.svg";
    } else {
        adminNav.className = "adminLeftNav leftNavDefault";
        updownBtn.src = "../images/down_black.svg";
    }
}