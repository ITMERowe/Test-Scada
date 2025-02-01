// Toggle sidebar visibility (expand/collapse) on button click
document.getElementById('menu-toggle').addEventListener('click', function () {
    document.getElementById('sidebar').classList.toggle('active');
    document.getElementById('page-content-wrapper').classList.toggle('active');

    // Fix: Ensure sidebar links show when expanded, hide when collapsed
    const sidebarLinks = document.querySelectorAll('.sidebar-links');

    if (document.getElementById('sidebar').classList.contains('active')) {
        sidebarLinks.forEach(link => {
            link.style.opacity = "0";  // Hide when collapsed
            link.style.pointerEvents = "none";
        });
    } else {
        sidebarLinks.forEach(link => {
            link.style.opacity = "1";  // Show when expanded
            link.style.pointerEvents = "auto";
        });
    }
});
