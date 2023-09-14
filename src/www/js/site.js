//Add the uris for your aks and aks hybrid services
const uris = ['https://localhost:7279/ServiceAPI/message', 'https://SERVER-IP/ServiceAPI/message'];

function LoadSelect() {
    uris.forEach((element) => {
        var option = document.createElement('option');
        option.text = element;
        document.getElementById('uri-select').add(option);
    });
}

function sendMessage() {
    var senderId = document.getElementById('sender-id').value.trim();
    var senderMsg = document.getElementById('sender-msg').value.trim();

    if ((senderId == '') || (senderMsg == '')) { alert ('Please enter a sender ID and message.'); return; }

    var uri = document.getElementById('uri-select').value;
    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: '{"senderId": "' + senderId + '","value": "' + senderMsg + '"}'
    })
        .then(response => response.json())
        .then(data => {
            console.log(data);
            document.getElementById('server-rsp').value = uri + ":\n" + JSON.stringify(data, null, 2) + "\n\n" + document.getElementById('server-rsp').value;
        })
        .catch(error => alert('Unable to send message to ' + uri + '.\n' + error));
}