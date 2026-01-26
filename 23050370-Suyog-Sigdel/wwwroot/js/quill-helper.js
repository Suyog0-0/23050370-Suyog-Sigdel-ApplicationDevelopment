window.quillEditors = {};

window.initQuill = (editorId, dotNetRef) => {
    const container = document.getElementById(editorId);
    if (!container) {
        console.error('Quill container not found:', editorId);
        return false;
    }

    const quill = new Quill(container, {
        theme: 'snow',
        modules: {
            toolbar: [
                ['bold', 'italic', 'underline'],
                [{ 'header': [1, 2, 3, false] }],
                [{ 'list': 'ordered' }, { 'list': 'bullet' }],
                ['link'],
                ['clean']
            ]
        },
        placeholder: 'Write your journal entry here...'
    });

    // Store the editor instance
    window.quillEditors[editorId] = quill;

    // Notify Blazor on content change
    quill.on('text-change', () => {
        if (dotNetRef) {
            dotNetRef.invokeMethodAsync('HandleContentChange', quill.root.innerHTML);
        }
    });

    return true;
};

window.getQuillContent = (editorId) => {
    const quill = window.quillEditors[editorId];
    return quill ? quill.root.innerHTML : '';
};

window.setQuillContent = (editorId, html) => {
    const quill = window.quillEditors[editorId];
    if (quill) {
        quill.root.innerHTML = html || '';
    }
};

window.clearQuill = (editorId) => {
    const quill = window.quillEditors[editorId];
    if (quill) {
        quill.setText('');
    }
};