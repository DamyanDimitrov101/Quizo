document.addEventListener('DOMContentLoaded', function () {

	var messageInput = document.getElementById('message');
	var id = document.getElementById('groupId');

	messageInput.focus();

	var route = '/chat';
	var connection = new signalR.HubConnectionBuilder()
		.withUrl(route)
		.build();

	connection.on('broadcastMessage', function (name, message) {
		var encodedName = name;
		var encodedMsg = message;

		var liElement = document.createElement('li');
		liElement.innerHTML = '<strong>' + encodedName + '</strong>:&nbsp;&nbsp;' + encodedMsg;
		document.getElementById('discussion').appendChild(liElement);
	});

	connection.start()
		.then(function () {
			console.log('connection started');

			connection
				.invoke("JoinGroup", id.value)
				.catch(err => console.error(err));

			var groupName = document.getElementById("nameGroup");
			document.getElementById('sendmessage').addEventListener('click', function (event) {
				connection.invoke('send', messageInput.value, groupName.value);

				messageInput.value = '';
				messageInput.focus();
				event.preventDefault();
			});
		})
		.catch(error => {
			console.error(error.message);
		});
});