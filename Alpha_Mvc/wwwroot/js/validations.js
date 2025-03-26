document.addEventListener("DOMContentLoaded", () => {
    const form = document.querySelector("form");
    if (!form) return;

    const fields = form.querySelectorAll("input[data-val='true']");

    fields.forEach(field => {
        field.addEventListener("input", () => {
            validateFields(field);
        });
    });
});

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