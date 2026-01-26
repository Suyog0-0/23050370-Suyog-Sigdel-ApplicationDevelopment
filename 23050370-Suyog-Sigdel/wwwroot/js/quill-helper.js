window.quillEditors = {};

window.initQuill = (editorId, dotNetRef) => {
    console.log("[Quill] initQuill:", editorId);

    const container = document.getElementById(editorId);
    if (!container) {
        console.error("Quill container not found:", editorId);
        return false;
    }

    const quill = new Quill(container, {
        theme: 'snow',
        placeholder: 'Write your journal entry here...',
        modules: {
            toolbar: [
                ['bold', 'italic', 'underline'],
                [{ 'script': 'sub' }, { 'script': 'super' }], // ✅ NEW
                [{ header: [1, 2, 3, false] }],
                [{ list: 'ordered' }, { list: 'bullet' }],
                ['link'],
                ['clean']
            ]
        }
    });

    // Store editor
    window.quillEditors[editorId] = quill;

    // Notify Blazor on change
    quill.on('text-change', () => {
        dotNetRef?.invokeMethodAsync(
            'HandleContentChange',
            quill.root.innerHTML
        );
    });

    //  links clickable
    quill.root.addEventListener('click', (e) => {
        const link = e.target.closest('a');
        if (link && link.href) {
            e.preventDefault();
            e.stopPropagation();
            window.open(link.href, '_blank');
        }
    });

    console.log("[Quill] Editor ready with sub & super script");
    return true;
};

window.getQuillContent = (editorId) =>
    window.quillEditors[editorId]?.root.innerHTML || '';

window.setQuillContent = (editorId, html) => {
    const q = window.quillEditors[editorId];
    if (q) q.root.innerHTML = html || '';
};

window.clearQuill = (editorId) => {
    const q = window.quillEditors[editorId];
    if (q) q.setText('');
};
