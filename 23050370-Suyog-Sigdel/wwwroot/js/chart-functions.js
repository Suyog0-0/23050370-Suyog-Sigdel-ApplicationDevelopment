// Store charts
let charts = {};

window.createCharts = function (
    moodLabels, moodData,
    tagLabels, tagData,
    freqLabels, freqData,
    pieLabels, pieData,
    dates, words
) {
    // Destroy old charts
    Object.values(charts).forEach(chart => chart && chart.destroy());
    charts = {};

    const colors = ['#3498db', '#e74c3c', '#f39c12', '#27ae60', '#9b59b6', '#1abc9c', '#e67e22', '#16a085'];

    // 1. Primary Moods
    const ctx1 = document.getElementById('moodChart');
    if (ctx1) {
        charts.mood = new Chart(ctx1, {
            type: 'doughnut',
            data: {
                labels: moodLabels,
                datasets: [{
                    data: moodData,
                    backgroundColor: colors
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    legend: { position: 'bottom' }
                }
            }
        });
    }

    // 2. Top Tags
    const ctx2 = document.getElementById('tagsChart');
    if (ctx2) {
        charts.tags = new Chart(ctx2, {
            type: 'bar',
            data: {
                labels: tagLabels,
                datasets: [{
                    label: 'Count',
                    data: tagData,
                    backgroundColor: '#3498db'
                }]
            },
            options: {
                responsive: true,
                plugins: { legend: { display: false } },
                scales: { y: { beginAtZero: true } }
            }
        });
    }

    // 3. All Moods
    const ctx3 = document.getElementById('frequentMoodsChart');
    if (ctx3) {
        charts.freq = new Chart(ctx3, {
            type: 'bar',
            data: {
                labels: freqLabels,
                datasets: [{
                    label: 'Count',
                    data: freqData,
                    backgroundColor: '#9b59b6'
                }]
            },
            options: {
                indexAxis: 'y',
                responsive: true,
                plugins: { legend: { display: false } },
                scales: { x: { beginAtZero: true } }
            }
        });
    }

    // 4. Tag Distribution
    const ctx4 = document.getElementById('tagBreakdownChart');
    if (ctx4) {
        charts.pie = new Chart(ctx4, {
            type: 'pie',
            data: {
                labels: pieLabels,
                datasets: [{
                    data: pieData,
                    backgroundColor: colors
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    legend: { position: 'bottom' }
                }
            }
        });
    }

    // 5. Word Count
    const ctx5 = document.getElementById('wordCountChart');
    if (ctx5) {
        charts.words = new Chart(ctx5, {
            type: 'line',
            data: {
                labels: dates,
                datasets: [{
                    label: 'Words',
                    data: words,
                    borderColor: '#27ae60',
                    backgroundColor: 'rgba(39, 174, 96, 0.2)',
                    fill: true
                }]
            },
            options: {
                responsive: true,
                scales: { y: { beginAtZero: true } }
            }
        });
    }
};

window.destroyCharts = function () {
    Object.values(charts).forEach(chart => chart && chart.destroy());
    charts = {};
};