//document.addEventListener('DOMContentLoaded', () => {
    function initFormValidation(modal) {
        console.log("initFormValidation");
        if (!modal) return;

        if (typeof (errorMessages !== 'undefined')) {
            Object.keys(errorMessages).forEach(key => {
                let span = modal.querySelector(`[data-valmsg-for='${key}']`);
                if (span) {
                    let errorText = errorMessages[key].join(', ');
                    span.innerText = errorText;
                    span.classList.add('field-validation-error');
                }
            });
        }


        const forms = modal.querySelectorAll('form');
        if (!forms) return;

        forms.forEach(form => {
            const inputs = form.querySelectorAll("input[data-val='true']");

            if (!inputs) return;

            inputs.forEach(input => {
                input.addEventListener('input', () => {
                    validateInputs(input);
                });
            });

            if (!form.dataset.listenerAttached) {
                form.addEventListener('submit', async () => {
                    const submitButton = form.querySelector("button[type='submit']");
                    submitButton.disabled = true;

                    clearErrorMessages(form);

                    try {
                        const formData = new FormData(form);

                        const res = await fetch(form.action, {
                            method: 'post',
                            body: formData
                        });

                        if (!res.ok) {
                            const data = await res.json();
                            Object.keys(data.errors).forEach(key => {
                                let input = form.querySelector(`[name='${key}']`);

                                if (input)
                                    input.classList.add('input-validation-error');

                                let inputError = form.querySelector(`[data-valmsg-for='${key}']`);

                                if (inputError) {
                                    inputError.innerText = data.errors[key].join('\n');
                                    inputError.classList.add('field-validation-error');
                                }
                            });
                        }
                    }
                    catch {
                        console.warn("Error occured submitting form.");
                    }

                    form.querySelector("button[type='submit']").disabled = false;
                });
                form.dataset.listenerAttached = 'true';
            }
        });
    };




    function clearErrorMessages(form) {
        form.querySelectorAll('[data-val="true]').forEach(input => {
            input.classList.remove('input-validation-error');
        });

        form.querySelectorAll('[data-valmsg-for]').forEach(field => {
            field.classList.remove('field-validation-error');
            field.innerText = "";
        });
    };



    function validateInputs(input) {
        let span = document.querySelector(`span[data-valmsg-for='${input.name}']`);
        if (!span) return;

        let errorMessage = "";
        let value = input.value.trim();

        if (input.hasAttribute('data-val-required') && value === "")
            errorMessage = input.getAttribute('data-val-required');

        if (input.hasAttribute('data-val-regex') && value !== "") {
            let patternAttr = input.getAttribute('data-val-regex-pattern');
            if (patternAttr) {
                try {
                    let regexPattern = new RegExp(patternAttr);
                    if (!regexPattern.test(value))
                        errorMessage = input.getAttribute('data-val-regex');
                } catch (e) {
                    console.warn(`Ogiltigt RegExp-mönster för ${input.name}:`, patternAttr, e);
                }
            } else {
                console.warn(`Regex-attribut saknas eller är tomt på fältet ${input.name}`);
            }
        }

        if (errorMessage) {
            input.classList.add("input-validation-error");
            span.classList.remove("field-validation-valid");
            span.classList.add("field-validation-error");
            span.textContent = errorMessage;
        } else {
            input.classList.remove("input-validation-error");
            span.classList.remove("field-validation-error");
            span.classList.add("field-validation-valid");
            span.textContent = "";
        }
    };
//});