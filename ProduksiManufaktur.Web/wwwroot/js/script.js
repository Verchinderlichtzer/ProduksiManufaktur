window.downloadFileFromStream = async (fileName, contentStreamReference) => {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
    URL.revokeObjectURL(url);
}

window.updateUrl = () => {
    let sections = document.querySelectorAll('section');
    let navLinks = document.querySelectorAll('.mud-timeline .mud-timeline-item .mud-timeline-item-divider .mud-timeline-item-dot .mud-timeline-item-dot-inner');

    window.onscroll = () => {
        sections.forEach(sec => {
            let top = window.scrollY;
            let offset = sec.offsetTop - 150;
            let height = sec.offsetHeight;
            let id = sec.getAttribute('id');

            if (top >= offset && top < offset + height) {
                navLinks.forEach(link => {
                    link.classList.remove('mud-timeline-dot-primary');
                    document.querySelector(`.mud-timeline .mud-timeline-item.${id} .mud-timeline-item-divider .mud-timeline-item-dot .mud-timeline-item-dot-inner`).classList.add('mud-timeline-dot-primary');
                })
            }
        })
    }
}

function scrollToSection(id) {
    var element = document.getElementById(id);
    element.scrollIntoView({ behavior: "smooth", block: "start", inline: "nearest" });
}