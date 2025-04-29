
document.addEventListener('DOMContentLoaded', () => {

    // Open edit project modal
    const editProjectModal = document.querySelector('#editProjectModal');
    const modalButtons = document.querySelectorAll('[data-modal="true"]');

    const circle = document.querySelector("#edit-proj-circle");
    const imagePreview = document.querySelector("#edit-proj-image-preview");

    //const modal = document.getElementById("editProjectModal");
    //const closeModalBtn = document.getElementById("closeModalButton");

    //if (modal && closeModalBtn) {
    //    document.querySelectorAll(".option-edit").forEach(button => {
    //        button.addEventListener("click", () => {
    //            modal.classList.remove("hidden");

    //            modal.querySelector('[name="Id"]').value = button.dataset.id;
    //            modal.querySelector('[name="ProjectTitle"]').value = button.dataset.title;
    //            modal.querySelector('[name="Budget"]').value = button.dataset.budget;
    //            modal.querySelector('[name="StartDate"]').value = button.dataset.start;
    //            modal.querySelector('[name="EndDate"]').value = button.dataset.end;
    //        });
    //    });

    //    closeModalBtn.addEventListener("click", () => {
    //        modal.classList.add("hidden");
    //    });
    //} else {
    //    console.error("Modal or close button not found in the DOM.");
    //}

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
                        editProjectModal.classList.add('show');
                        circle.classList.remove('show');
                        imagePreview.classList.add('show');
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

    // Open modal
    //const modalButtons = document.querySelectorAll('[data-modal="true"]');

    //modalButtons.forEach(button => {
    //    button.addEventListener('click', (e) => {
    //        e.stopPropagation();

    //        const target = button.getAttribute('data-target');
    //        const modal = document.querySelector(target);

    //        if (modal) {
    //            modal.classList.toggle('show');
    //        }
    //    });
    //});



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

        document.querySelector('[name="Id"]').value = project.id;
        document.querySelector('[name="ProjectName"]').value = project.projectTitle;
        document.querySelector('[name="ProjectBudget"]').value = project.budget;
        document.querySelector('[name="ProjectStartDate"]').value = formatDate(project.startDate);
        document.querySelector('[name="ProjectEndDate"]').value = formatDate(project.endDate);
        document.querySelector('[name="ProjectDescription"]').value = project.description;
        document.querySelector('[name="ClientsName"]').value = project.clientName;
        document.querySelector('#edit-proj-image-preview').src = project.imageUrl;
    };


    // Populate preselected members on editProject
    function populateMembers(data) {
        console.log('Data received:', data);
        const preSelected = data.members.map(member => ({
            id: member.id,
            fullName: `${member.firstName} ${member.lastName}`,
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
