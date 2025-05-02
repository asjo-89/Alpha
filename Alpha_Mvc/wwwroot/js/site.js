
window.setupCountdown = function (id, endDate, startDate) {

    const dueDate = new Date(endDate);
    const startingDate = new Date(startDate);
    const dateNow = Date.now();

    const span = document.querySelector(`#countDown_${id}`);

    if (isNaN(dueDate) || isNaN(startingDate)) {
        console.warn("Invalid date format.");
        return;
    }

    CountDown();

    function CountDown() {

        const timeLeft = dueDate - dateNow;
        const timeToStart = startingDate - dateNow;

        const daysToStart = Math.floor(timeToStart / (1000 * 60 * 60 * 24));
        const weeksToStart = Math.floor(daysToStart / 7);

        const days = Math.floor(timeLeft / (1000 * 60 * 60 * 24));
        const weeks = Math.floor(days / 7);

        if (daysToStart >= 0 && daysToStart <= 6) {
            span.innerText = `Starts in ${Math.abs(daysToStart)} day(s)`;
        }
        else if (daysToStart >= 0 && daysToStart >= 7) {
            span.innerText = `Starts in ${Math.abs(weeksToStart)} week(s)`;
        }

        else if (days < 7 && days >= 0) {
            span.innerText = `${days} day(s) left`;
        }
        else if (days >= 7) {
            span.innerText = `${weeks} week(s) left`;
        }
        else if (days < 0 && days >= -6) {
            span.innerText = `${Math.abs(days)} day(s) overdue`;
        }
        else if (days <= -7) {
            span.innerText = `${Math.abs(weeks)} week(s) overdue`;
        }
    }
};      

document.addEventListener('DOMContentLoaded', () => {

    // Open edit project modal
    const modalButtons = document.querySelectorAll('[data-modal="true"]');
    const projectModal = document.querySelector('[data-project-id]');
    let editProjectModal = null;

    const circle = document.querySelector("#edit-proj-circle");
    const imagePreview = document.querySelector("#edit-proj-image-preview");

    if (projectModal) {
        const projectId = projectModal.getAttribute('data-project-id');
        editProjectModal = document.querySelector(`#editProjectModal_${projectId}`);

        if (editProjectModal) {
            const imageInput = editProjectModal.querySelector('#edit-proj-image-input');
            const currentUrlField = editProjectModal.querySelector('#current-url');

            if (imageInput && currentUrlField) {
                imageInput.addEventListener('change', (e) => {
                const image = e.target.files[0];
                    if (image) {
                        currentUrlField.value = "";
                    }
                });
            }
        }
    }

    modalButtons.forEach(button => {
        button.addEventListener('click', (e) => {
            e.stopPropagation();

            const target = button.getAttribute('data-target');
            const modal = document.querySelector(target);


            if (e.target.matches('#edit-project-button')) {
                clearForm(modal);
                const projectId = e.target.dataset.projectId;

                fetch(`/Project/EditProject?id=${projectId}`)
                    .then(response => {
                        if (!response.ok)
                            return console.error(`HTTP error! Status: ${response.status}`);
                        return response.json()
                    })
                    .then(data => {
                        console.log('Data received from server:', data);
                        populateFields(data.project);
                        populateMembers(data);

                        if (editProjectModal) {
                            editProjectModal.classList.add('show');
                            circle.classList.remove('show');
                            imagePreview.classList.add('show');
                        }
                        else {
                            console.error('editProjectModal was not found');
                        }
                    })
                    .catch(error => console.error('Error getting data for project:', error));
            }
            else {
                clearForm(modal);
                if (modal) {
                    modal.classList.add('show');
                } else {
                    console.error('Modal not found for target:', target);
                }
            }
        });
    });


    // Dropdown menu cards
    document.addEventListener('click', (e) => {
        const menuBtns = document.querySelectorAll('#menu-button');
        const menu = document.querySelectorAll('.dropdown-content');

        // Open dropdown
        menuBtns.forEach(button => {
            const dropDown = button.nextElementSibling;

            // Close other dropdowns if another is clicked
            if (dropDown && button.contains(e.target)) {
                menu.forEach(click => {

                    if (click !== dropDown) {
                        click.classList.remove('show');
                    }
                })
            }

            dropDown.classList.toggle('show');
        });

        // Close dropdown when clicking outside or on button
        menu.forEach(dropDown => {

            if (!dropDown.contains(e.target) && !dropDown.previousElementSibling.contains(e.target)) {
                dropDown.classList.remove('show');
            }
        });
    });


    // Populate fields in modal for edit project
    function populateFields(project) {
        if (!project) {
            console.error('No project data received.');
            return;
        }

        console.log('Project data received:', project);

        const formatDate = (dateString) => {
            const date = new Date(dateString);
            if (isNaN(date)) return ''; 
            return date.toISOString().split('T')[0];
        };
        //const startDateField = document.querySelector('[name="StartDate"]');
        //console.log('StartDate field:', startDateField);

        //console.log(formatDate(project.startDate));

        editProjectModal.querySelector('[name="Id"]').value = project.id;
        editProjectModal.querySelector('[name="ProjectTitle"]').value = project.projectTitle;
        console.error(document.querySelector('[name="ProjectTitle"]').value);
        editProjectModal.querySelector('[name="Budget"]').value = project.budget;
        editProjectModal.querySelector('[name="StartDate"]').value = formatDate(project.startDate);
        editProjectModal.querySelector('[name="EndDate"]').value = formatDate(project.endDate);
        editProjectModal.querySelector('[name="Description"]').value = project.description;
        editProjectModal.querySelector('[name="ClientName"]').value = project.clientName;
        editProjectModal.querySelector('[name="CurrentUrl"]').value = project.imageUrl;
        editProjectModal.querySelector('#edit-proj-image-preview').src = project.imageUrl;
    };


    // Populate preselected members on editProject
    function populateMembers(data) {
        console.log('Data received:', data);
        const preSelected = data.members.map(member => ({
            id: member.id,
            fullName: `${member.firstName} ${member.lastName}`,
            email: member.email,
            imageUrl: member.imageUrl
        }));
        console.log('Preselected before initTagSelector:', preSelected);

        initTagSelector({
            containerId: 'tag-members-edit',
            inputId: 'member-search-edit',
            resultsId: 'member-results-edit',
            searchUrl: (query) => `/Member/SearchMembers?term=${encodeURIComponent(query)}`,
            displayName: 'fullName',
            displayEmail: 'email',
            imageProperty: 'imageUrl',
            tagClass: 'member-edit',
            emptyMessage: 'No members found.',
            preSelected: preSelected
        }); 
    }

    function clearForm(modal) {
        const form = modal.querySelector('form');
        const inputs = form.querySelectorAll('input, textarea, select');
        let counter = 1;
        inputs.forEach(input => {
            if (!input.value) {
                input.value = "";
                console.log("input cleared", counter);

            }
            counter++;
        });
    }
});
