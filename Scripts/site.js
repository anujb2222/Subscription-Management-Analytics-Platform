
var sublyticCharts = {};

$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();

    $('.custom-alert').delay(5000).fadeOut(400);
});

function destroyChartIfExists(chartId) {
    if (sublyticCharts[chartId] && typeof sublyticCharts[chartId].destroy === 'function') {
        sublyticCharts[chartId].destroy();
        delete sublyticCharts[chartId];
    }
    var existing = document.getElementById(chartId);
    if (existing) {
        var parent = existing.parentNode;
        var newCanvas = document.createElement('canvas');
        newCanvas.id = chartId;
        newCanvas.height = existing.height;
        parent.replaceChild(newCanvas, existing);
    }
}

function getGradient(ctx, color1, color2) {
    var gradient = ctx.createLinearGradient(0, 0, 0, 400);
    gradient.addColorStop(0, color1);
    gradient.addColorStop(1, color2);
    return gradient;
}

function initDashboardCharts(monthlyTrend, spendingByCategory, billingCycles) {
    if (monthlyTrend) initMonthlyTrendChart(monthlyTrend);
    if (spendingByCategory) initCategoryChart(spendingByCategory);
    if (billingCycles) initBillingCycleChart(billingCycles);
}

function initMonthlyTrendChart(data) {
    destroyChartIfExists('monthlyTrendChart');
    var ctx = document.getElementById('monthlyTrendChart');
    if (!ctx) return;
    var ctx2d = ctx.getContext('2d');

    var labels = Object.keys(data);
    var values = Object.values(data);

    var gradient = getGradient(ctx2d, 'rgba(102, 126, 234, 0.35)', 'rgba(102, 126, 234, 0.0)');

    sublyticCharts.monthlyTrendChart = new Chart(ctx2d, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: 'Monthly Spend ($)',
                data: values,
                borderColor: '#667eea',
                backgroundColor: gradient,
                borderWidth: 3,
                fill: true,
                tension: 0.4,
                pointBackgroundColor: '#ffffff',
                pointBorderColor: '#667eea',
                pointBorderWidth: 2,
                pointRadius: 5,
                pointHoverRadius: 7,
                pointHoverBackgroundColor: '#764ba2',
                pointHoverBorderColor: '#ffffff',
                pointHoverBorderWidth: 2
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false
                },
                tooltip: {
                    backgroundColor: '#2d3436',
                    titleFont: { size: 13, weight: 'bold' },
                    bodyFont: { size: 13 },
                    padding: 12,
                    cornerRadius: 8,
                    displayColors: false,
                    callbacks: {
                        label: function (context) {
                            return 'Spend: $' + context.parsed.y.toFixed(2);
                        }
                    }
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    grid: {
                        color: 'rgba(0,0,0,0.05)',
                        drawBorder: false
                    },
                    ticks: {
                        color: '#636e72',
                        font: { size: 11, weight: '500' },
                        callback: function (value) {
                            return '$' + value;
                        }
                    }
                },
                x: {
                    grid: { display: false, drawBorder: false },
                    ticks: {
                        color: '#636e72',
                        font: { size: 11, weight: '500' }
                    }
                }
            },
            interaction: {
                intersect: false,
                mode: 'index'
            }
        }
    });
}

function initCategoryChart(data) {
    destroyChartIfExists('categoryChart');
    var ctx = document.getElementById('categoryChart');
    if (!ctx) return;
    var ctx2d = ctx.getContext('2d');

    var labels = Object.keys(data);
    var values = Object.values(data);
    var colors = [
        '#667eea', '#f093fb', '#4facfe', '#43e97b',
        '#fa709a', '#ffd700', '#00f2fe', '#a8edea',
        '#ff9a9e', '#fbc2eb'
    ];
    var bgColors = labels.map(function (_, i) { return colors[i % colors.length]; });

    sublyticCharts.categoryChart = new Chart(ctx2d, {
        type: 'doughnut',
        data: {
            labels: labels,
            datasets: [{
                data: values,
                backgroundColor: bgColors,
                borderColor: '#ffffff',
                borderWidth: 3,
                hoverOffset: 8
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            cutout: '65%',
            plugins: {
                legend: {
                    position: 'bottom',
                    labels: {
                        padding: 15,
                        usePointStyle: true,
                        pointStyle: 'circle',
                        font: { size: 11, weight: '500' },
                        color: '#2d3436'
                    }
                },
                tooltip: {
                    backgroundColor: '#2d3436',
                    titleFont: { size: 13, weight: 'bold' },
                    bodyFont: { size: 13 },
                    padding: 12,
                    cornerRadius: 8,
                    callbacks: {
                        label: function (context) {
                            var total = context.dataset.data.reduce(function (a, b) { return a + b; }, 0);
                            var pct = ((context.parsed / total) * 100).toFixed(1);
                            return context.label + ': $' + context.parsed.toFixed(2) + ' (' + pct + '%)';
                        }
                    }
                }
            }
        }
    });
}

function initBillingCycleChart(data) {
    destroyChartIfExists('billingCycleChart');
    var ctx = document.getElementById('billingCycleChart');
    if (!ctx) return;
    var ctx2d = ctx.getContext('2d');

    var labels = Object.keys(data);
    var values = Object.values(data);

    sublyticCharts.billingCycleChart = new Chart(ctx2d, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: 'Subscriptions',
                data: values,
                backgroundColor: [
                    'rgba(162, 155, 254, 0.8)',
                    'rgba(102, 126, 234, 0.8)',
                    'rgba(9, 132, 227, 0.8)',
                    'rgba(253, 203, 110, 0.8)',
                    'rgba(0, 184, 148, 0.8)'
                ],
                borderColor: [
                    '#a29bfe', '#667eea', '#0984e3', '#fdcb6e', '#00b894'
                ],
                borderWidth: 2,
                borderRadius: 6,
                borderSkipped: false
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: { display: false },
                tooltip: {
                    backgroundColor: '#2d3436',
                    padding: 12,
                    cornerRadius: 8,
                    callbacks: {
                        label: function (ctx) {
                            return ctx.parsed.y + ' subscription(s)';
                        }
                    }
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        precision: 0,
                        color: '#636e72',
                        font: { size: 11, weight: '500' }
                    },
                    grid: {
                        color: 'rgba(0,0,0,0.05)',
                        drawBorder: false
                    }
                },
                x: {
                    grid: { display: false, drawBorder: false },
                    ticks: {
                        color: '#636e72',
                        font: { size: 10, weight: '500' }
                    }
                }
            }
        }
    });
}

function initAnalyticsCharts(yearlyTrend, quarterlySpending, categoryBreakdown) {
    if (yearlyTrend) initYearlyTrendChart(yearlyTrend);
    if (quarterlySpending) initQuarterlyChart(quarterlySpending);
    if (categoryBreakdown && categoryBreakdown.length) {
        initAnalyticsCategoryChart(categoryBreakdown);
    }
}

function initYearlyTrendChart(data) {
    destroyChartIfExists('yearlyTrendChart');
    var ctx = document.getElementById('yearlyTrendChart');
    if (!ctx) return;
    var ctx2d = ctx.getContext('2d');

    var labels = Object.keys(data);
    var values = Object.values(data);
    var gradient = getGradient(ctx2d, 'rgba(0, 184, 148, 0.35)', 'rgba(0, 184, 148, 0.0)');

    sublyticCharts.yearlyTrendChart = new Chart(ctx2d, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: 'Monthly Spend ($)',
                data: values,
                borderColor: '#00b894',
                backgroundColor: gradient,
                borderWidth: 3,
                fill: true,
                tension: 0.35,
                pointBackgroundColor: '#ffffff',
                pointBorderColor: '#00b894',
                pointBorderWidth: 2,
                pointRadius: 4,
                pointHoverRadius: 6
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: { display: false },
                tooltip: {
                    backgroundColor: '#2d3436',
                    padding: 12,
                    cornerRadius: 8,
                    callbacks: {
                        label: function (ctx) {
                            return 'Spend: $' + ctx.parsed.y.toFixed(2);
                        }
                    }
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    grid: {
                        color: 'rgba(0,0,0,0.05)',
                        drawBorder: false
                    },
                    ticks: {
                        color: '#636e72',
                        font: { size: 11 },
                        callback: function (v) { return '$' + v; }
                    }
                },
                x: {
                    grid: { display: false, drawBorder: false },
                    ticks: {
                        color: '#636e72',
                        font: { size: 10 },
                        maxRotation: 45,
                        minRotation: 30
                    }
                }
            },
            interaction: { intersect: false, mode: 'index' }
        }
    });
}

function initQuarterlyChart(data) {
    destroyChartIfExists('quarterlyChart');
    var ctx = document.getElementById('quarterlyChart');
    if (!ctx) return;
    var ctx2d = ctx.getContext('2d');

    var labels = Object.keys(data);
    var values = Object.values(data);

    sublyticCharts.quarterlyChart = new Chart(ctx2d, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: 'Quarterly Spend ($)',
                data: values,
                backgroundColor: function (ctx) {
                    var colors = [
                        'rgba(102, 126, 234, 0.75)',
                        'rgba(240, 147, 251, 0.75)',
                        'rgba(79, 172, 254, 0.75)',
                        'rgba(67, 233, 123, 0.75)'
                    ];
                    return colors[ctx.dataIndex % colors.length];
                },
                borderColor: ['#667eea', '#f093fb', '#4facfe', '#43e97b'],
                borderWidth: 2,
                borderRadius: 10,
                borderSkipped: false
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: { display: false },
                tooltip: {
                    backgroundColor: '#2d3436',
                    padding: 12,
                    cornerRadius: 8,
                    callbacks: {
                        label: function (ctx) {
                            return 'Total: $' + ctx.parsed.y.toFixed(2);
                        }
                    }
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        color: '#636e72',
                        font: { size: 11 },
                        callback: function (v) { return '$' + v; }
                    },
                    grid: {
                        color: 'rgba(0,0,0,0.05)',
                        drawBorder: false
                    }
                },
                x: {
                    grid: { display: false, drawBorder: false },
                    ticks: {
                        color: '#636e72',
                        font: { size: 11, weight: '500' }
                    }
                }
            }
        }
    });
}

function initAnalyticsCategoryChart(breakdown) {
    destroyChartIfExists('analyticsCategoryChart');
    var ctx = document.getElementById('analyticsCategoryChart');
    if (!ctx) return;
    var ctx2d = ctx.getContext('2d');

    var labels = breakdown.map(function (b) { return b.CategoryName; });
    var values = breakdown.map(function (b) { return b.MonthlyAmount; });
    var colors = breakdown.map(function (b) { return b.CategoryColor || '#667eea'; });

    sublyticCharts.analyticsCategoryChart = new Chart(ctx2d, {
        type: 'polarArea',
        data: {
            labels: labels,
            datasets: [{
                data: values,
                backgroundColor: colors.map(function (c) { return c + 'CC'; }),
                borderColor: colors,
                borderWidth: 2
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'right',
                    labels: {
                        padding: 12,
                        usePointStyle: true,
                        pointStyle: 'rectRounded',
                        font: { size: 11, weight: '500' }
                    }
                },
                tooltip: {
                    backgroundColor: '#2d3436',
                    padding: 12,
                    cornerRadius: 8,
                    callbacks: {
                        label: function (ctx) {
                            var total = ctx.dataset.data.reduce(function (a, b) { return a + b; }, 0);
                            var pct = ((ctx.parsed.r / total) * 100).toFixed(1);
                            return ctx.label + ': $' + ctx.parsed.r.toFixed(2) + '/mo (' + pct + '%)';
                        }
                    }
                }
            },
            scales: {
                r: {
                    beginAtZero: true,
                    ticks: {
                        backdropColor: 'transparent',
                        color: '#636e72',
                        font: { size: 10 }
                    },
                    grid: { color: 'rgba(0,0,0,0.05)' },
                    angleLines: { color: 'rgba(0,0,0,0.05)' }
                }
            }
        }
    });
}

function initSubscriptionPage() {
    $('.status-toggle').on('change', function () {
        var checkbox = $(this);
        var id = checkbox.data('id');
        var isChecked = checkbox.prop('checked');
        var badge = checkbox.closest('td,div').find('.badge-success, .badge-secondary');

        $.ajax({
            url: '/Subscription/ToggleActive',
            type: 'POST',
            data: { id: id },
            success: function (res) {
                if (res && res.success) {
                    badge.removeClass('badge-success badge-secondary');
                    badge.addClass(res.isActive ? 'badge-success' : 'badge-secondary');
                    badge.text(res.isActive ? 'Active' : 'Inactive');
                    showToast(res.isActive ? 'Subscription activated' : 'Subscription marked as inactive', 'success');
                }
            },
            error: function () {
                checkbox.prop('checked', !isChecked);
                showToast('Failed to update status', 'danger');
            }
        });
    });

    $('.btn-renew').on('click', function () {
        var btn = $(this);
        var id = btn.data('id');

        btn.prop('disabled', true).html('<i class="fas fa-spinner fa-spin mr-2"></i>Processing...');

        $.ajax({
            url: '/Subscription/Renew',
            type: 'POST',
            data: { id: id },
            success: function (res) {
                if (res && res.success) {
                    showToast('Renewal date updated to ' + res.nextRenewalDate, 'success');
                    setTimeout(function () { location.reload(); }, 1200);
                } else {
                    btn.prop('disabled', false).html('<i class="fas fa-redo mr-2"></i>Mark Renewed');
                    showToast('Failed to update renewal', 'danger');
                }
            },
            error: function () {
                btn.prop('disabled', false).html('<i class="fas fa-redo mr-2"></i>Mark Renewed');
                showToast('Failed to update renewal', 'danger');
            }
        });
    });
}

function initSubscriptionForm() {
    var priceInput = $('input[name="Price"], input#Price');
    var cycleSelect = $('select[name="BillingCycle"], select#BillingCycle');

    function updateCostPreview() {
        var price = parseFloat(priceInput.val()) || 0;
        var cycleStr = cycleSelect.val();
        var cycles = { 'Weekly': 0, 'Monthly': 1, 'Quarterly': 3, 'SemiAnnually': 6, 'Semi-Annually': 6, 'Annually': 12 };
        var cycle = cycles[cycleStr];
        if (cycle === undefined) cycle = 1;

        var weekly, monthly, annual;
        if (cycle === 0) { weekly = price; monthly = price * 4.33; annual = monthly * 12; }
        else if (cycle === 1) { weekly = price / 4.33; monthly = price; annual = price * 12; }
        else { monthly = price / cycle; weekly = monthly / 4.33; annual = monthly * 12; }

        var html = '';
        if (price > 0) {
            html = '' +
                '<div class="row text-center">' +
                    '<div class="col-6 mb-3">' +
                        '<div class="small text-muted">Weekly Equivalent</div>' +
                        '<div class="font-weight-bold text-secondary">$' + weekly.toFixed(2) + '</div>' +
                    '</div>' +
                    '<div class="col-6 mb-3">' +
                        '<div class="small text-muted">Monthly Equivalent</div>' +
                        '<div class="font-weight-bold text-primary">$' + monthly.toFixed(2) + '</div>' +
                    '</div>' +
                '</div>' +
                '<div class="col-12 mb-3 p-3 bg-light rounded text-center">' +
                    '<div class="small text-muted mb-1">Annual Projected Cost</div>' +
                    '<div class="h3 mb-0 text-success font-weight-bold">$' + annual.toFixed(2) + '</div>' +
                '</div>';
        } else {
            html = '<p class="text-muted text-center mb-0">Fill in price and billing cycle to see cost breakdown</p>';
        }
        $('#costPreview').html(html);
    }

    if (priceInput.length && cycleSelect.length) {
        updateCostPreview();
        priceInput.on('input', updateCostPreview);
        cycleSelect.on('change', updateCostPreview);
    }

    var startDateInput = $('input[name="StartDate"], input#StartDate');
    var renewalInput = $('input[name="NextRenewalDate"], input#NextRenewalDate');

    if (startDateInput.length && renewalInput.length && !renewalInput.val()) {
        startDateInput.on('change', function () {
            var startVal = $(this).val();
            if (startVal && !renewalInput.val()) {
                var cycles = { 'Weekly': 7, 'Monthly': 1, 'Quarterly': 3, 'SemiAnnually': 6, 'Semi-Annually': 6, 'Annually': 12 };
                var cycle = cycles[cycleSelect.val()];
                var dt = new Date(startVal);
                if (cycle === 7) {
                    dt.setDate(dt.getDate() + 7);
                } else {
                    var c = cycle || 1;
                    dt.setMonth(dt.getMonth() + c);
                }
                renewalInput.val(dt.toISOString().split('T')[0]);
            }
        });
    }
}

function showToast(message, type) {
    type = type || 'info';
    var alertClass = {
        'success': 'alert-success',
        'danger': 'alert-danger',
        'warning': 'alert-warning',
        'info': 'alert-info'
    }[type] || 'alert-info';

    var iconClass = {
        'success': 'fa-check-circle',
        'danger': 'fa-exclamation-triangle',
        'warning': 'fa-exclamation-circle',
        'info': 'fa-info-circle'
    }[type] || 'fa-info-circle';

    var toastHtml = '' +
        '<div class="alert ' + alertClass + ' alert-dismissible fade show custom-alert toast-notification" role="alert" style="position:fixed;top:90px;right:20px;z-index:9999;max-width:380px;min-width:280px;">' +
            '<i class="fas ' + iconClass + ' mr-2"></i> ' + message +
            '<button type="button" class="close" data-dismiss="alert"><span>&times;</span></button>' +
        '</div>';

    $('body').append(toastHtml);
    $('.toast-notification').delay(4000).fadeOut(400, function () { $(this).remove(); });
}
