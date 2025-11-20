// Admin JavaScript Functions

// Sidebar Toggle
document.addEventListener('DOMContentLoaded', function() {
    const sidebarToggle = document.getElementById('sidebarToggle');
    const sidebar = document.getElementById('sidebar');
    
    if (sidebarToggle) {
        sidebarToggle.addEventListener('click', function() {
            sidebar.classList.toggle('collapsed');
        });
    }

    // Set active nav link
    const currentPath = window.location.pathname;
    document.querySelectorAll('.sidebar-nav .nav-link').forEach(link => {
        if (link.getAttribute('href') === currentPath) {
            link.classList.add('active');
        }
    });
});

// Toast Notification
function showToast(title, message, type = 'success') {
    const toast = document.getElementById('toast');
    const toastTitle = document.getElementById('toastTitle');
    const toastMessage = document.getElementById('toastMessage');
    
    toastTitle.textContent = title;
    toastMessage.textContent = message;
    
    // Remove existing type classes
    toast.classList.remove('bg-success', 'bg-danger', 'bg-warning', 'bg-info');
    
    // Add type class
    if (type === 'success') {
        toast.classList.add('bg-success');
    } else if (type === 'error') {
        toast.classList.add('bg-danger');
    } else if (type === 'warning') {
        toast.classList.add('bg-warning');
    } else {
        toast.classList.add('bg-info');
    }
    
    const bsToast = new bootstrap.Toast(toast);
    bsToast.show();
}

// Show Loading
function showLoading() {
    const overlay = document.createElement('div');
    overlay.className = 'spinner-overlay';
    overlay.id = 'loadingOverlay';
    overlay.innerHTML = '<div class="spinner"></div>';
    document.body.appendChild(overlay);
}

// Hide Loading
function hideLoading() {
    const overlay = document.getElementById('loadingOverlay');
    if (overlay) {
        overlay.remove();
    }
}

// AJAX Form Submit
function submitForm(formId, successCallback) {
    const form = document.getElementById(formId);
    if (!form) return;
    
    form.addEventListener('submit', function(e) {
        e.preventDefault();
        
        // Validate CategoryId for book forms
        if (formId === 'bookForm') {
            const categorySelect = form.querySelector('select[name="CategoryId"]');
            if (categorySelect && (!categorySelect.value || categorySelect.value === '' || categorySelect.value === '0')) {
                showToast('Lỗi', 'Vui lòng chọn thể loại sách.', 'error');
                categorySelect.focus();
                return false;
            }
        }
        
        // Check HTML5 validation
        if (!form.checkValidity()) {
            form.reportValidity();
            return false;
        }
        
        showLoading();
        
        const formData = new FormData(form);
        
        // FormData will automatically include the anti-forgery token from the form
        // No need to add it to headers when using FormData
        
        fetch(form.action, {
            method: 'POST',
            body: formData,
            headers: {
                'X-Requested-With': 'XMLHttpRequest',
                'Accept': 'application/json'
                // Don't set Content-Type - let browser set it with boundary for FormData
            }
        })
        .then(response => {
            if (!response.ok) {
                // If response is not OK, try to get error message
                return response.text().then(text => {
                    let errorMsg = `HTTP ${response.status}: ${response.statusText}`;
                    try {
                        const json = JSON.parse(text);
                        if (json.message) errorMsg = json.message;
                    } catch (e) {
                        // Not JSON, use status text
                    }
                    throw new Error(errorMsg);
                });
            }
            
            const contentType = response.headers.get('content-type');
            if (contentType && contentType.includes('application/json')) {
                return response.json();
            } else {
                return response.text().then(text => {
                    throw new Error('Server trả về HTML thay vì JSON. Có thể do lỗi validation hoặc exception.');
                });
            }
        })
        .then(data => {
            hideLoading();
            if (data.success) {
                showToast('Thành công', data.message, 'success');
                if (successCallback) {
                    successCallback(data);
                }
                // Close modal if exists
                const modal = bootstrap.Modal.getInstance(form.closest('.modal'));
                if (modal) modal.hide();
                // Redirect if provided, otherwise reload
                if (data.redirectUrl) {
                    setTimeout(() => window.location.href = data.redirectUrl, 1000);
                } else {
                    setTimeout(() => location.reload(), 1000);
                }
            } else {
                showToast('Lỗi', data.message || 'Có lỗi xảy ra', 'error');
            }
        })
        .catch(error => {
            hideLoading();
            console.error('Form submit error:', error);
            let errorMessage = 'Có lỗi xảy ra khi gửi dữ liệu.';
            if (error.message) {
                errorMessage = error.message;
            } else if (error.name === 'TypeError' && error.message.includes('fetch')) {
                errorMessage = 'Không thể kết nối đến server. Vui lòng kiểm tra kết nối mạng.';
            }
            showToast('Lỗi', errorMessage, 'error');
        });
    });
}

// Delete Confirmation
function confirmDelete(message, deleteUrl, token) {
    if (confirm(message)) {
        showLoading();
        
        fetch(deleteUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': token
            }
        })
        .then(response => response.json())
        .then(data => {
            hideLoading();
            if (data.success) {
                showToast('Thành công', data.message, 'success');
                setTimeout(() => location.reload(), 1000);
            } else {
                showToast('Lỗi', data.message, 'error');
            }
        })
        .catch(error => {
            hideLoading();
            showToast('Lỗi', 'Có lỗi xảy ra: ' + error.message, 'error');
        });
    }
}

// Open Modal
function openModal(modalId) {
    const modal = new bootstrap.Modal(document.getElementById(modalId));
    modal.show();
}

// Close Modal
function closeModal(modalId) {
    const modal = bootstrap.Modal.getInstance(document.getElementById(modalId));
    if (modal) modal.hide();
}

// Initialize tooltips
document.addEventListener('DOMContentLoaded', function() {
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function(tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
});

// Handle AJAX responses globally
$(document).ajaxSuccess(function(event, xhr, settings) {
    if (xhr.responseJSON) {
        if (xhr.responseJSON.success) {
            showToast('Thành công', xhr.responseJSON.message, 'success');
        } else {
            showToast('Lỗi', xhr.responseJSON.message, 'error');
        }
    }
});

$(document).ajaxError(function(event, xhr, settings, thrownError) {
    showToast('Lỗi', 'Có lỗi xảy ra khi xử lý yêu cầu', 'error');
});

