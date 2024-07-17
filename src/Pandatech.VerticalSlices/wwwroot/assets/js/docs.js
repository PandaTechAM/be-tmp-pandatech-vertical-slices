document.addEventListener('DOMContentLoaded', function () {

    var link = document.querySelector("link[rel*='icon']") || document.createElement("link");
    document.head.removeChild(link);
    link = document.createElement("link");
    link.type = "image/x-icon";
    link.rel = "shortcut icon";
    link.href = "../assets/images/favicon.svg";
    document.getElementsByTagName("head")[0].appendChild(link);

    // Adjusted MutationObserver code
    const observer = new MutationObserver((mutations) => {
        const modal = document.querySelector('.modal-ux-content');
        if (modal) {
            modal.scrollTo(0, 0);
            observer.disconnect();
        }
    });

    observer.observe(document.body, {
        childList: true,
        subtree: true,
    });
});