﻿window.FileProcessor = {
    apiUrl: "api/fileuploader/uploadfile",

    clearMessages: function () {
        var successMessageElement = document.getElementById("SuccessMessage");
        successMessageElement.innerText = '';

        var errorMessageElement = document.getElementById("ErrorMessage");
        errorMessageElement.innerText = '';
    },

    writeSuccessMessage: function (message) {
        this.clearMessages();
        var successMessageElement = document.getElementById("SuccessMessage");
        successMessageElement.innerText = message;
    },

    writeErrorMessage: function (message) {
        this.clearMessages();
        var errorMessageElement = document.getElementById("ErrorMessage");
        errorMessageElement.innerText = message;
    },

    isFormValid: function () {
        var fileInput = document.getElementById("FileInput");
        if (fileInput && fileInput.value) {
            return true;
        }

        return false;
    },

    clearInput: function () {
        var fileInput = document.getElementById("FileInput");
        if (fileInput && fileInput.value) {
            fileInput.value = '';
        }
    },

    uploadFileToServer: function () {
        var self = this;

        var fileInput = document.getElementById("FileInput");
        var file = fileInput.files[0];

        var xhr = new XMLHttpRequest();
        
        xhr.onreadystatechange = function (e) {
            if (4 === this.readyState) {
                if (e.target.status === 200)
                {
                    let result = JSON.parse(e.target.response);
                    self.writeSuccessMessage(`Success. Processed ${result.processedTransactions} transactions.`);
                }

                if (e.target.status === 400) {
                    let result = JSON.parse(e.target.response);
                    self.writeErrorMessage(`BadRequest. \n ${result.error}`);
                }

                self.clearInput();
            }
        };
        xhr.open('post', this.apiUrl, true);
        

        var formData = new FormData();
        formData.append("File", file);

        xhr.send(formData);
    },

    addSubmitButtonHandler: function (e) {

        if (this.isFormValid()) {
            this.clearMessages();
            this.uploadFileToServer();
        }
        else {
            this.writeErrorMessage("Please select any file.");
        }
    },

    turnDragAndDropOn: function () {
        let dropArea = document.querySelector('body');

        ['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
            dropArea.addEventListener(eventName, function (e) {
                e.preventDefault();
                e.stopPropagation();
            }, false);
        });

        ['dragenter', 'dragover'].forEach(eventName => {
            dropArea.addEventListener(eventName, function () {
                dropArea.classList.add('highlight');
            }, false);
        });

        ['dragleave', 'drop'].forEach(eventName => {
            dropArea.addEventListener(eventName, function () {
                dropArea.classList.remove('highlight');
            }, false);
        });

        dropArea.addEventListener('drop', function (e) {
            let dt = e.dataTransfer;
            document.getElementById("FileInput").files = dt.files;
        }, false);
    },

    domReady: function () {
        var self = this;
        document.getElementById("SumbitButton").addEventListener("click", function (e) {
            self.addSubmitButtonHandler();
        });

        this.turnDragAndDropOn();
    }
};

document.addEventListener('DOMContentLoaded', function () {
    FileProcessor.domReady();
});