// Store Quill instances
window.quillInstances = {};

// Initialize Quill editor
window.initQuillEditor = function (editorId) {
    try {
        const container = document.getElementById(editorId);
        if (!container) {
            console.error('Container not found:', editorId);
            return;
        }

        const quill = new Quill(container, {
            theme: 'snow',
            placeholder: 'Write your journal entry here...',
            modules: {
                toolbar: [
                    ['bold', 'italic', 'underline'],
                    [{ 'header': [1, 2, 3, false] }],
                    [{ 'list': 'ordered' }, { 'list': 'bullet' }],
                    ['link'],
                    ['clean']
                ],
                history: {
                    delay: 1000,
                    maxStack: 50,
                    userOnly: true
                }
            }
        });

        // Store instance
        window.quillInstances[editorId] = quill;

        // Make entire editor clickable
        const editor = container.querySelector('.ql-editor');
        if (editor) {
            editor.style.cursor = 'text';
        }

        console.log('Quill initialized:', editorId);
    } catch (error) {
        console.error('Quill init error:', error);
    }
};

// Get HTML content
window.getQuillHTML = function (editorId) {
    const quill = window.quillInstances[editorId];
    if (!quill) return '';
    return quill.root.innerHTML || '';
};

// Set HTML content - FIXED to prevent reverting
window.setQuillHTML = function (editorId, html) {
    const quill = window.quillInstances[editorId];
    if (!quill) return;

    // Disable history tracking while setting content
    quill.history.clear();

    // Set content using Delta for better control
    if (html && html.trim()) {
        quill.root.innerHTML = html;
    } else {
        quill.setText('');
    }

    // Clear history after setting to prevent undo reverting to old content
    setTimeout(() => {
        quill.history.clear();
    }, 100);
};

// Clear editor - FIXED
window.clearQuillEditor = function (editorId) {
    const quill = window.quillInstances[editorId];
    if (!quill) return;

    // Clear content
    quill.setText('');

    // Clear history to prevent undo
    quill.history.clear();
};

// Destroy editor
window.destroyQuillEditor = function (editorId) {
    const quill = window.quillInstances[editorId];
    if (quill) {
        // Clear everything before destroying
        quill.setText('');
        quill.history.clear();
    }
    delete window.quillInstances[editorId];
};
