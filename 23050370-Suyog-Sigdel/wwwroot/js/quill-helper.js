window.quillEditors = {};

window.initQuill = (editorId, dotNetRef) => {
    console.log("[Quill] initQuill called:", editorId);

    const container = document.getElementById(editorId);
    if (!container) {
        console.error("[Quill] Container NOT found:", editorId);
        return false;
    }

    console.log("[Quill] Container found");

    const quill = new Quill(container, {
        theme: 'snow',
        placeholder: 'Write your journal entry here...',
        modules: {
            toolbar: [
                ['bold', 'italic', 'underline'],
                [{ header: [1, 2, 3, false] }],
                [{ list: 'ordered' }, { list: 'bullet' }],
                ['link'],
                ['clean']
            ]
        }
    });

    console.log("[Quill] Editor initialized");

    window.quillEditors[editorId] = quill;

    quill.on('text-change', () => {
        console.log("[Quill] Content changed");
        dotNetRef?.invokeMethodAsync(
            'HandleContentChange',
            quill.root.innerHTML
        );
    });

    return true;
};

window.getQuillContent = (editorId) => {
    console.log("[Quill] getQuillContent:", editorId);
    return window.quillEditors[editorId]?.root.innerHTML || '';
};

window.setQuillContent = (editorId, html) => {
    console.log("[Quill] setQuillContent:", editorId);
    const q = window.quillEditors[editorId];
    if (q) q.root.innerHTML = html;
};

window.clearQuill = (editorId) => {
    console.log("[Quill] clearQuill:", editorId);
    const q = window.quillEditors[editorId];
    if (q) q.setText('');
};
