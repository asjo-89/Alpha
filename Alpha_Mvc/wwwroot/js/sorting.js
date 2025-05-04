function sortProjects() {

    const tabContainer = document.getElementById('filter-bar');
    var children = Array.from(tabContainer.children);

    tabContainer.addEventListener('click', (e) => {

        if (!e.target.matches('button'))
            return;

        const status = e.target.id.toLowerCase();
        const projects = Array.from(document.querySelectorAll('#project-card'));

        const result = projects.filter(filterProjects(status));

        if (status === 'all') {
            children.forEach(child => child.classList.remove('active'));
            e.target.classList.add('active');

            projects.forEach(card => {
                card.style.display = 'flex';
            })
        }
        else {
            children.forEach(child => child.classList.remove('active'));
            e.target.classList.add('active');
            updateDisplayCards(projects, result);
        }
    })
};

function updateDisplayCards(projects, result) {
    projects.forEach(card => card.style.display = 'none');
    result.forEach(card => card.style.display = 'flex');
};

function filterProjects(status) {
    return (projectCard) => projectCard.dataset.status === status;
};
