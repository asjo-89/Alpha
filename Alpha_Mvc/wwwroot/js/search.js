function initTagSelector(config) {
    let activeIndex = -1;
    let selectedIds = [];

    const tagContainer = document.getElementById(config.containerId);
    const input = document.getElementById(config.inputId);
    const results = document.getElementById(config.resultsId);

    if (Array.isArray(config.preselected)) {
        config.preselected.forEach(item => addTag(item));
    }

    input.addEventListener('focus', () => {
        results.classList.add('search-focus');
    });

    input.addEventListener('blur', () => {
        setTimeout(() => {
            results.classList.remove('search-focus');
        }, 100);
    });

    input.addEventListener('input', () => {
        const query = input.value.trim();
        activeIndex = -1;

        if (query.length === 0) {
            results.style.display = 'none';
            results.innerHTML = '';
            return;
        }

        fetch(config.searchUrl(query))
            .then(response => response.json())
            .then(data => renderSearchResults(data));
    });

    input.addEventListener('keydown', (e) => {
        const items = results.querySelectorAll('.search-item');

        switch (e.key) {
            case 'ArrowDown':
                e.preventDefault();
                if (items.length > 0) {
                    activeIndex = (activeIndex + 1) % items.length;
                    updateActiveItem(items);
                }
                break;

            case 'ArrowUp':
                e.preventDefault();
                if (items.length > 0) {
                    activeIndex = (activeIndex - 1) % items.length;
                    updateActiveItem(items);
                }
                break;

            case 'Enter':
                e.preventDefault();
                if (activeIndex >= 0 && items[activeIndex]) {
                    items[activeIndex].click();
                }
                break;

            case 'Backspace':
                if (input.value === '') {
                    removeLastTag();
                }
                break;
        };
    });

   
    function addTag(item) {
        const id = String(item.id);
        if (selectedIds.includes(id))
            return;

        selectedIds.push(id);
        updateSelectedIdsInput();

        const tag = document.createElement('span');
        tag.classList.add(config.tagClass || 'tag');

        if (Array.isArray(config.preselected)) {
            config.preselected.forEach(item => addTag(item));
        }


        if (config.tagClass === 'tag') {
            tag.innerHTML = `
                <span>${item[config.displayName]}</span>
            `;
        }
        else if (config.tagClass === 'member') {
            tag.innerHTML = `
                <img class="profile-img" src="${item[config.imageProperty]}">
                <span>${item[config.displayName]}</span>
            `;
        }

        const deleteBtn = document.createElement('span');
        deleteBtn.textContent = 'x';
        deleteBtn.classList.add('btn-delete-search');
        deleteBtn.dataset.id = id;
        deleteBtn.addEventListener('click', (e) => {
            e.stopPropagation();
            selectedIds = selectedIds.filter(i => i !== id);
            tag.remove();
            updateSelectedIdsInput();
        });

        tag.appendChild(deleteBtn);
        tagContainer.insertBefore(tag, input);
        input.value = '';
        results.innerHTML = '';
        results.style.display = 'none';
    };


    function removeLastTag() {
        const tags = tagContainer.querySelectorAll(`.${config.tagClass}`);
        if (tags.length === 0)
            return;

        const lastTag = tags[tag.length - 1];
        const lastId = String(lastTag.querySelector('.btn-delete-search').dataset.id);

        selectedIds = selectedIds.filter(id => id !== lastId);
        lastTag.remove();
        updateSelectedIdsInput();
    };


    function updateSelectedIdsInput() {
        const hideInput = document.getElementById('SelectedIds');
        if (hideInput) {
            hideInput.value = JSON.stringify(selectedIds);

            console.log("SelectedIds uppdaterat:", hideInput.value);
        }
    };


    function updateActiveItem(items) {
        items.forEach(item => item.classList.remove('active'));
        if (items[activeIndex]) {
            items[activeIndex].classList.add('active');
            items[activeIndex].scrollIntoView({ block: 'nearest' });
        }
    };


    function renderSearchResults(data) {
        results.innerHTML = '';

        if (data.length === 0) {
            const noResult = document.createElement('div');
            noResult.classList.add('search-item');
            noResult.textContent = config.emptyMessage || 'No results.';
            results.appendChild(noResult);
        }
        else {
            data.forEach(item => {
                const id = String(item.id);
                if (!selectedIds.includes(id)) {
                    const resultItem = document.createElement('div');
                    resultItem.classList.add('search-item');
                    resultItem.dataset.id = item.id;

                    if (config.tagClass === 'tag') {
                        resultItem.innerHTML = `
                            <div class="tag-info">
                                <span>${item[config.displayName]}</span>
                                <span class="email">${item[config.displayEmail]}</span>
                            </div>
                        `;
                    }
                    else if (config.tagClass === 'member') {
                        resultItem.innerHTML = `
                            <img class="profile-img" src="${config.imageFolder || ''}${item[config.imageProperty]}">
                            <div class="tag-info">
                                <span>${item[config.displayName]}</span>
                                <span class="email">${item[config.displayEmail]}</span>
                            </div>
                        `;
                    }

                    resultItem.addEventListener('click', (e) => {
                        e.stopPropagation();
                        addTag(item)
                    });
                    results.appendChild(resultItem);
                }
            });
        }
        results.style.display = 'flex';
    };
}

