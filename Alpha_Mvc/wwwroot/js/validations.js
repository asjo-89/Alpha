document.addEventListener("DOMContentLoaded", () => {
    const modals = document.querySelectorAll('.modal');

    const form = document.querySelector("form");
    if (!form) return;

    
    modals.forEach(modal => {
        

        const form = modal.querySelector("form");

        const fields = form.querySelectorAll("input[data-val='true']");

        fields.forEach(field => {
            field.addEventListener("input", () => {
                validateFields(field);
            });
        });

        form.addEventListener('submit', async (e) => {
            e.preventDefault();

            clearErrorMessages(form);

            const formData = new FormData(form);

            try {
                const response = await fetch(form.action, {
                    method: 'post',
                    body: formData
                });

                if (!response.ok) {
                    const data = await response.json();

                    Object.keys(data.errors).forEach(key => {
                        let input = form.querySelector(`[name="${key}"]`);

                        if (input)
                            input.classList.add('input-validation-error');

                        let field = form.querySelector(`[data-valmsg-for="${key}"]`);

                        if (field) {
                            field.innerText = data.errors[key].join('\n');
                            field.classList.add('field-validation-error');
                        }
                    })
                }
                else
                    location.reload();
            }
            catch {
                console.log('Error when submitting the form.');
            }
        })

    })

    //form.forEach(form =>
    //    form.addEventListener('submit', async (e) => {
    //        e.preventDefault();

    //        clearErrorMessages(form);

    //        const formData = new FormData(form);

    //        try {
    //            const response = await fetch(form.action, {
    //                method: 'post',
    //                body: formData
    //            });

    //            if (!response.ok) {
    //                const data = await response.json();

    //                Object.keys(data.errors).forEach(key => {
    //                    let input = form.querySelector(`[name="${key}"]`);

    //                    if (input)
    //                        input.classList.add('input-validation-error');

    //                    let field = form.querySelector(`[data-valmsg-for="${key}"]`);

    //                    if (field) {
    //                        field.innerText = data.errors[key].join('\n');
    //                        field.classList.add('field-validation-error');
    //                    }
    //                })
    //            }
    //            else
    //                location.reload();
    //        }
    //        catch {
    //            console.log('Error when submitting the form.');
    //        }
    //    })
    //);
});


function clearErrorMessages(form) {
    form.querySelectorAll('[data-val="true]').forEach(input => {
        input.classList.remove('input-validation-error');
    });

    form.querySelectorAll('[data-valmsg-for]').forEach(field => {
        field.classList.remove('field-validation-error');
        field.innerText = "";
    })
};


function validateFields(field) {
    let span = document.querySelector(`span[data-valmsg-for='${field.name}']`);
    if (!span) return;

    let errorMsg = "";
    let value = field.value.trim();

    if (field.hasAttribute("data-val-required") && value === "") {
        errorMsg = field.getAttribute("data-val-required");
    };

    if (field.hasAttribute("data-val-regex") && value !== "") {
        let regexPattern = new RegExp(field.getAttribute("data-val-regex-pattern"));

        if (!regexPattern.test(value))
            errorMsg = field.getAttribute("data-val-regex");
    }

    if (errorMsg) {
        field.classList.add("input-validation-error");
        span.classList.remove("field-validation-valid");
        span.classList.add("field-validation-error");
        span.textContent = errorMsg;
    } else {
        field.classList.remove("input-validation-error");
        span.classList.remove("field-validation-error");
        span.classList.add("field-validation-valid");
        span.textContent = "";
    }
};