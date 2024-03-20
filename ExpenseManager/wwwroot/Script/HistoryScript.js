console.log("Test");

 document.getElementById('OkDelete').addEventListener('click', function(event) {        
        document.getElementById('DeleteModal').style.display = 'none';
        document.getElementById('Overlay').style.display = 'none';        
    });

    document.getElementById('closeEditImade').addEventListener('click', function(event) {
         document.getElementById('DeleteModal').style.display = 'none';
         document.getElementById('Overlay').style.display = 'none';        
     })

    document.getElementById('CancelDelete').addEventListener('click', function(event) {
        document.getElementById('DeleteModal').style.display = 'none';
        document.getElementById('Overlay').style.display = 'none';        
    });

    

    document.getElementById('closeEdit').addEventListener('click', function(event) {
        document.querySelector('.edit-modal').style.display = 'none';
            document.getElementById('Overlay').style.display = 'none';         
    })

    document.getElementById('closeEditImage').addEventListener('click', function(event) {
        document.querySelector('.edit-modal').style.display = 'none';
        document.getElementById('Overlay').style.display = 'none';         
    })





let timeout;

   document.querySelectorAll('.hidden').forEach(hiddenElement => {
    hiddenElement.addEventListener('mouseover', function(event) {
        clearTimeout(timeout);
        const rect = hiddenElement.getBoundingClientRect();
        const itemId = hiddenElement.getAttribute('data-item-id'); // Отримуємо ID об'єкта
        document.getElementById('hiddenMessage').style.display = 'block';
        document.getElementById('hiddenMessage').style.top = rect.top+10 + 'px';
        document.getElementById('hiddenMessage').style.left = rect.right+100 + 'px';
        document.getElementById('itemId').value = itemId; // Зберігаємо ID обраного об'єкта у прихованому полі
    });

    hiddenElement.addEventListener('mouseout', function() {
        timeout = setTimeout(() => {
            document.getElementById('hiddenMessage').style.display = 'none';
        }, 200);
    });
});

    document.getElementById('hiddenMessage').addEventListener('mouseover', function() {
        clearTimeout(timeout);
    });

    document.getElementById('hiddenMessage').addEventListener('mouseout', function() {
        timeout = setTimeout(() => {
            document.getElementById('hiddenMessage').style.display = 'none';
        }, 200);
    });



    var fromCurrency = document.getElementById("CurrentyFrom")
    var select = document.getElementById("CurrentySelect")
    select.addEventListener('change', function(){
        fromCurrency.submit();
    })







 function handleButtonClick(button) {
            var itemId = button.id;
        // Записуємо id кнопки в приховане поле IdBody
        var IdBodyElement = document.getElementById('IdBody');
        if (IdBodyElement) {
            IdBodyElement.value = itemId;
        }
    }







