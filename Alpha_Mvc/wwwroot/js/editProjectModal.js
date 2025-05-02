
console.log("editProjectModal.js");






let camera = document.querySelector("#edit-camera-project");
const imageInput = document.querySelector("#edit-proj-image-input");

function initEdit() {
    if (!imageInput) return;

    camera.addEventListener("click", () => {
        imageInput.click();
    });

    imageInput.addEventListener("change", (e) => {
        const image = e.target.files[0];
        const circle = document.querySelector("#edit-proj-circle");
        const imagePreview = document.querySelector("#edit-proj-image-preview");

        if (image) {
            const reader = new FileReader();

            reader.onload = (e) => {
                imagePreview.src = e.target.result;
                circle.classList.remove('show');
                imagePreview.classList.add('show');
            };

            reader.readAsDataURL(image);
        }
        else {
            circle.classList.add('show');
            imagePreview.classList.remove('show');
        }
    });


    document.addEventListener('click', (e) => {
        const modals = document.querySelectorAll('.modal');
        modals.forEach(modal => {
            const modalContainer = modal.querySelector('.modal-container');
            const exitButton = modal.querySelector('[data-close="true"]');

            if (exitButton.contains(e.target)) {
                modal.classList.remove('show');

                modal.querySelectorAll('form').forEach(form => {
                    form.reset();

                    const imagePreview = form.querySelector('#edit-proj-image-preview');
                    if (imagePreview) {
                        imagePreview.src = '';
                        imagePreview.classList.remove('show');
                    }

                    const container = form.querySelector('#edit-proj-circle');
                    if (container) {
                        container.classList.add('show');
                    }

                });
            }
        });
    });
}

initEdit();