
document.addEventListener('DOMContentLoaded', () => {
    const forms = document.querySelectorAll('form');
    if (!forms) return;

    forms.forEach(form => {
        const inputs = form.querySelectorAll("input[data-val='true']");
        const pswInput = form.querySelector('#password');

        if (!inputs || form.hasAttribute('data-no-validation')) return;

        inputs.forEach(input => {
            input.addEventListener('input', () => {
                if (input.id === "checkbox") return;

                validateInputs(input);
            });
        });

        if (pswInput) {
            pswInput.addEventListener('input', () => {
                validatePswRealTime(pswInput);
            })
        }

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
    })




    function clearErrorMessages(form) {
        form.querySelectorAll('[data-val="true]').forEach(input => {
            input.classList.remove('input-validation-error');
        });

        form.querySelectorAll('[data-valmsg-for]').forEach(field => {
            field.classList.remove('field-validation-error');
            field.innerText = "";
        })
    };



    function validateInputs(input) {
        let span = document.querySelector(`span[data-valmsg-for='${input.name}']`);
        if (!span) return;

        const isPswSpan = span.id === "psw-validation-signin" || span.id === "psw-validation-create";


        let errorMessage = "";
        let value = input.value.trim();

        if (input.hasAttribute('data-val-required') && value === "")
            errorMessage = input.getAttribute('data-val-required');

        if (input.hasAttribute('data-val-regex') && value !== "" && !isPswSpan) {
            let regexPattern = new RegExp(input.getAttribute('data-val-regex-pattern'));

            if (!regexPattern.test(value))
                errorMessage = input.getAttribute('data-val-regex');
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
    }



    function validatePswRealTime(input) {
        const value = input.value;
        const rulesUl = document.querySelector('#rules-list-signin') || document.querySelector('#rules-list-create');

        const rules = {
            length: /.{8}/,
            upper: /[A-Z]/,
            lower: /[a-z]/,
            digit: /\d/,
            special: /[#?!@$%^&*-]/
        };

        Object.entries(rules).forEach(([key, regex]) => {
            rulesUl.style.display = "flex";
            const listItem = document.querySelector(`#${rulesUl.id} li[data-rule='${key}']`);
           
            if (!listItem) return;

            if (regex.test(value)) {
                listItem.style.display = "none";
            }
            else {
                listItem.style.display = "list-item";
            }
        });
    };
});