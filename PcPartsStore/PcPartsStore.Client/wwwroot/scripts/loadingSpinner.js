
    window.loadingSpinner = {
        show: function () {
            const spinner = document.getElementById('loading-spinner');
    if (spinner) {
        spinner.style.display = 'flex';
            }
        },
    hide: function () {
            const spinner = document.getElementById('loading-spinner');
    if (spinner) {
        spinner.style.display = 'none';
            }
        }
    };

