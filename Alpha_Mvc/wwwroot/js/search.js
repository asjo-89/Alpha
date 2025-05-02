function initTagSelector(config) {
    let activeIndex = -1;
    let selectedIds = [];

    const tagContainer = document.getElementById(config.containerId);
    const input = document.getElementById(config.inputId);
    const results = document.getElementById(config.resultsId);
    
    clearTags();

    if (Array.isArray(config.preSelected)) {
        console.log('Preselected members:', config.preSelected);
        config.preSelected.forEach(item => {
            addTag(item);
        });
    } else {
        console.error('Preselected is not an array:', config.preSelected);
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
        console.log("SelectedIds" + { selectedIds });

        if (selectedIds.includes(id))
            return;

        selectedIds.push(id);

        if (config.containerId == 'tag-members-edit') {
            updateSelectedIdsInputEdit();
        }
        else {
            updateSelectedIdsInput();
        }

        const tag = document.createElement('span');
        tag.classList.add(config.tagClass || 'tag');

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
        else if (config.tagClass === 'member-edit') {
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

            if (containerId == 'tag-members-edit') {
                updateSelectedIdsInputEdit();
            }
            else {
                updateSelectedIdsInput();
            }
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

        if (containerId == 'tag-members-edit') {
            updateSelectedIdsInputEdit();
        }
        else {
            updateSelectedIdsInput();
        }
    };


    function updateSelectedIdsInput() {
        const hiddenInput = document.getElementById('SelectedIds');
        if (hiddenInput) {
            hiddenInput.value = JSON.stringify(selectedIds);

            console.log("SelectedIds uppdaterat:", hiddenInput.value);
        }
    };

    function updateSelectedIdsInputEdit() {
        const hiddenInput = document.getElementById('SelectedIdsEdit');
        if (hiddenInput) {
            hiddenInput.value = JSON.stringify(selectedIds);

            console.log("SelectedIdsEdit uppdaterat:", hiddenInput.value);
        }
    };


    function updateActiveItem(items) {
        items.forEach(item => item.classList.remove('active'));
        if (items[activeIndex]) {
            items[activeIndex].classList.add('active');
            items[activeIndex].scrollIntoView({ block: 'nearest' });
        }
    };

    function clearTags() {
        tagContainer.querySelectorAll(`.${config.tagClass}`).forEach(tag => tag.remove());
        selectedIds = [];
        updateSelectedIdsInput();
    }


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
                    else if (config.tagClass === 'member-edit') {
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

