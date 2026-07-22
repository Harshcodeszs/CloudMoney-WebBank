function confirmDeposit() {
    Swal.fire({
        title: 'Confirm Deposit',
        text: "Are you sure you want to deposit this amount?",
        icon: 'info',
        showCancelButton: true,
        confirmButtonColor: '#4A90E2',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Deposit it!'
    }).then((result) => {
        if (result.isConfirmed) {
            // If they click yes, trigger the actual ASP.NET button click
            __doPostBack('<%= btnDeposit.UniqueID %>', '');
        }
    });
    return false; // Prevents the page from refreshing immediately
}

function toggleVisibility(controlId, icon) {
    var passwordInput = document.getElementById(controlId);

    if (passwordInput.type === "password") {
        passwordInput.type = "text";
        icon.classList.remove("fa-eye");
        icon.classList.add("fa-eye-slash");
    } else {
        passwordInput.type = "password";
        icon.classList.remove("fa-eye-slash");
        icon.classList.add("fa-eye");
    }
}
