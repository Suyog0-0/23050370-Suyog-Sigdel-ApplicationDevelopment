// Store chart instances
let moodChartInstance = null;
let tagsChartInstance = null;
let frequentMoodsChartInstance = null;
let tagBreakdownChartInstance = null;
let wordCountChartInstance = null;

window.createCharts = function (
    moodLabels, moodData,
    tagLabels, tagData,
    frequentMoodLabels, frequentMoodData,
    tagBreakdownLabels, tagBreakdownData,
    dateLabels, wordCounts
) {
    console.log("🎨 Creating all charts...");

    // Destroy existing charts
    if (moodChartInstance) moodChartInstance.destroy();
    if (tagsChartInstance) tagsChartInstance.destroy();
    if (frequentMoodsChartInstance) frequentMoodsChartInstance.destroy();
    if (tagBreakdownChartInstance) tagBreakdownChartInstance.destroy();
    if (wordCountChartInstance) wordCountChartInstance.destroy();

    // Common colors
    const colors = ['#3498db', '#e74c3c', '#f39c12', '#27ae60', '#9b59b6', '#1abc9c', '#e67e22', '#16a085'];

    // Chart defaults for dark mode
    Chart.defaults.color = '#e0e0e0';
    Chart.defaults.borderColor = 'rgba(255, 255, 255, 0.1)';

    // 1. Primary Mood Distribution (Doughnut Chart)
    const moodCtx = document.getElementById('moodChart');
    if (moodCtx) {
        moodChartInstance = new Chart(moodCtx, {
            type: 'doughnut',
            data: {
                labels: moodLabels,
                datasets: [{
                    data: moodData,
                    backgroundColor: colors,
                    borderColor: '#2a2a2a',
                    borderWidth: 3
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: true,
                plugins: {
                    legend: {
                        position: 'bottom',
                        labels: {
                            color: '#e0e0e0',
                            padding: 15,
                            font: { size: 13 }
                        }
                    },
                    tooltip: {
                        callbacks: {
                            label: function(context) {
                                return context.label + ': ' + context.parsed + ' entries';
                            }
                        }
                    }
                }
            }
        });
        console.log("✅ Primary mood chart created");
    } else {
        console.log("❌ ERROR: moodChart canvas not found!");
    }

    // 2. Top 5 Tags (Bar Chart)
    const tagsCtx = document.getElementById('tagsChart');
    if (tagsCtx) {
        tagsChartInstance = new Chart(tagsCtx, {
            type: 'bar',
            data: {
                labels: tagLabels,
                datasets: [{
                    label: 'Count',
                    data: tagData,
                    backgroundColor: '#3498db',
                    borderColor: '#2980b9',
                    borderWidth: 2,
                    borderRadius: 5
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: true,
                plugins: {
                    legend: { display: false }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        ticks: {
                            color: '#e0e0e0',
                            stepSize: 1
                        },
                        grid: {
                            color: 'rgba(255, 255, 255, 0.1)'
                        }
                    },
                    x: {
                        ticks: {
                            color: '#e0e0e0'
                        },
                        grid: {
                            display: false
                        }
                    }
                }
            }
        });
        console.log("✅ Tags bar chart created");
    } else {
        console.log("❌ ERROR: tagsChart canvas not found!");
    }

    // 3. All Moods Frequency (Bar Chart - Horizontal)
    const frequentMoodsCtx = document.getElementById('frequentMoodsChart');
    if (frequentMoodsCtx) {
        frequentMoodsChartInstance = new Chart(frequentMoodsCtx, {
            type: 'bar',
            data: {
                labels: frequentMoodLabels,
                datasets: [{
                    label: 'Frequency',
                    data: frequentMoodData,
                    backgroundColor: '#9b59b6',
                    borderColor: '#8e44ad',
                    borderWidth: 2,
                    borderRadius: 5
                }]
            },
            options: {
                indexAxis: 'y', // Horizontal bars
                responsive: true,
                maintainAspectRatio: true,
                plugins: {
                    legend: { display: false }
                },
                scales: {
                    x: {
                        beginAtZero: true,
                        ticks: {
                            color: '#e0e0e0',
                            stepSize: 1
                        },
                        grid: {
                            color: 'rgba(255, 255, 255, 0.1)'
                        }
                    },
                    y: {
                        ticks: {
                            color: '#e0e0e0'
                        },
                        grid: {
                            display: false
                        }
                    }
                }
            }
        });
        console.log("✅ Frequent moods chart created");
    } else {
        console.log("❌ ERROR: frequentMoodsChart canvas not found!");
    }

    // 4. Tag Distribution (Pie Chart)
    const tagBreakdownCtx = document.getElementById('tagBreakdownChart');
    if (tagBreakdownCtx) {
        tagBreakdownChartInstance = new Chart(tagBreakdownCtx, {
            type: 'pie',
            data: {
                labels: tagBreakdownLabels,
                datasets: [{
                    data: tagBreakdownData,
                    backgroundColor: colors,
                    borderColor: '#2a2a2a',
                    borderWidth: 2
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: true,
                plugins: {
                    legend: {
                        position: 'bottom',
                        labels: {
                            color: '#e0e0e0',
                            padding: 12,
                            font: { size: 12 }
                        }
                    },
                    tooltip: {
                        callbacks: {
                            label: function(context) {
                                const total = context.dataset.data.reduce((a, b) => a + b, 0);
                                const percentage = ((context.parsed / total) * 100).toFixed(1);
                                return context.label + ': ' + context.parsed + ' (' + percentage + '%)';
                            }
                        }
                    }
                }
            }
        });
        console.log("✅ Tag breakdown chart created");
    } else {
        console.log("❌ ERROR: tagBreakdownChart canvas not found!");
    }

    // 5. Word Count Trend (Line Chart)
    const wordCountCtx = document.getElementById('wordCountChart');
    if (wordCountCtx) {
        wordCountChartInstance = new Chart(wordCountCtx, {
            type: 'line',
            data: {
                labels: dateLabels,
                datasets: [{
                    label: 'Words',
                    data: wordCounts,
                    borderColor: '#27ae60',
                    backgroundColor: 'rgba(39, 174, 96, 0.2)',
                    borderWidth: 3,
                    fill: true,
                    tension: 0.4,
                    pointBackgroundColor: '#27ae60',
                    pointBorderColor: '#fff',
                    pointBorderWidth: 2,
                    pointRadius: 5,
                    pointHoverRadius: 7
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: true,
                plugins: {
                    legend: {
                        display: true,
                        labels: {
                            color: '#e0e0e0',
                            font: { size: 13 }
                        }
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        ticks: {
                            color: '#e0e0e0'
                        },
                        grid: {
                            color: 'rgba(255, 255, 255, 0.1)'
                        }
                    },
                    x: {
                        ticks: {
                            color: '#e0e0e0'
                        },
                        grid: {
                            color: 'rgba(255, 255, 255, 0.05)'
                        }
                    }
                }
            }
        });
        console.log("✅ Word count chart created");
    } else {
        console.log("❌ ERROR: wordCountChart canvas not found!");
    }

    console.log("✅ All 5 charts created successfully!");
};

window.destroyCharts = function () {
    if (moodChartInstance) {
        moodChartInstance.destroy();
        moodChartInstance = null;
    }
    if (tagsChartInstance) {
        tagsChartInstance.destroy();
        tagsChartInstance = null;
    }
    if (frequentMoodsChartInstance) {
        frequentMoodsChartInstance.destroy();
        frequentMoodsChartInstance = null;
    }
    if (tagBreakdownChartInstance) {
        tagBreakdownChartInstance.destroy();
        tagBreakdownChartInstance = null;
    }
    if (wordCountChartInstance) {
        wordCountChartInstance.destroy();
        wordCountChartInstance = null;
    }
    console.log("🗑️ All charts destroyed");
};