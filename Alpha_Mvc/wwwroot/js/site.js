
document.addEventListener('DOMContentLoaded', () => {
    // Open modal
    const modalButtons = document.querySelectorAll('[data-modal="true"]');

    modalButtons.forEach(button => {
        button.addEventListener('click', (e) => {
            e.stopPropagation();

            const target = button.getAttribute('data-target');
            const modal = document.querySelector(target);
            //console.log(modal);
            //let projectId = button.getAttribute('data-project-id');
            //console.log(projectId)

            if (modal) {

                //if (projectId) {
                //    fetch(`/Project/EditProjectModal?id=${projectId}`)
                //        .then(response => response.text())
                //        .then(content => {
                //            document.querySelector("#modalContent").innerHTML = content;
                //            document.querySelector("#editProjectModal").classList.toggle("show");
                //        })
                //        .catch(error => console.error('Error loading modal:', error));
                //}
                //else {
                    modal.classList.toggle('show');
                //}
            }
        });
    });


    // Close modal by clicking outside
    //document.addEventListener('click', (e) => {
    //    const modals = document.querySelectorAll('.modal');

    //    modals.forEach(modal => {
    //        const modalContainer = modal.querySelector('.modal-container');
    //        const exitButton = modal.querySelector('[data-close="true"]');

    //        if (!modalContainer.contains(e.target) || exitButton.contains(e.target)) {
    //            modal.classList.remove('show');

    //            modal.querySelectorAll('form').forEach(form => {
    //                form.reset();

    //                const imagePreview = form.querySelector('#image-preview');
    //                if (imagePreview) {
    //                    imagePreview.src = '';
    //                    imagePreview.classList.remove('show');
    //                }

    //                const container = form.querySelector('#circle');
    //                if (container) 
    //                    container.classList.add('show');
    //            })
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

    const memberSelect = document.getElementById('member-select');
    if (memberSelect) {
        new Choices(memberSelect, {
            removeItemButton: true,
            placeholderValue: 'Select members...',
            searchPlaceholderValue: 'Search members',
            maxItemCount: -1,
            shouldSort: false
        });
    }

});




