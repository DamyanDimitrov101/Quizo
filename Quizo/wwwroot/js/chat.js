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
		var time = new Date().toLocaleTimeString();
		time = time.replace(/[^0-9:]/g, '');
		time = time.replace(/^0(?:0:0:)?/, '');

		var liElement = document.createElement('li');
		liElement.classList.add("d-flex");
		liElement.classList.add("justify-content-between");
		liElement.innerHTML = '<span style="font-weight:bold;">' + encodedName
			+ '</span><span style="width: 100%; padding-left: 3.8%;">' + encodedMsg
			+ '</span>' + '<span style="color:#00bcd4;">' + time + '</span>';
		document.getElementById('discussion').appendChild(liElement);

		$(`#discussion li:last-child`)
			.fadeOut(300)
			.fadeIn(300)
			.fadeOut(300)
			.fadeIn(300)
			.fadeOut(200)
			.fadeIn(200)
			.fadeOut(200)
			.fadeIn(200);
	});

	connection.start()
		.then(function () {
			console.log('connection started');

			connection
				.invoke("JoinGroup", id.value)
				.catch(err => console.error(err));

			var messageInput = document.getElementById("message");

			messageInput.addEventListener("keypress", function (event) {
				if (event.keyCode === 13) {
					event.preventDefault();

					document.getElementById("sendmessage").click();
				}
			});

			var groupName = document.getElementById("nameGroup");
			document.getElementById('sendmessage').addEventListener('click', function (event) {
				connection.invoke('send', messageInput.value, groupName.value, id.value);

				messageInput.value = '';
				messageInput.focus();
				event.preventDefault();
			});
		})
		.catch(error => {
			console.error(error.message);
		});
});