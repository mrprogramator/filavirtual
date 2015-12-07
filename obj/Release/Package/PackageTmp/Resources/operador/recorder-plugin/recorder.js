'use strict';

function uploadBlob(url, blob) {
    return new Promise(function (resolve, reject) {
        var xhr = new XMLHttpRequest();
        xhr.open('POST', url, true);

        xhr.onload = function () {
            resolve(xhr.response)
        };

        xhr.onerror = reject;

        xhr.send(blob);
    });
}

function downloadBlob(url) {
    return new Promise(function (resolve, reject) {
        var xhr = new XMLHttpRequest();
        xhr.open('GET', url, true);
        xhr.responseType = 'blob';

        xhr.onload = function () {
            resolve(xhr.response)
        };

        xhr.onerror = reject;

        xhr.send();
    });
}

function getAudioStream() {
    var getUserMedia = navigator.getUserMedia
                      || navigator.webkitGetUserMedia
                      || navigator.mozGetUserMedia
                      || navigator.msGetUserMedia;

    return new Promise(function (resolve, reject) {
        getUserMedia.call(navigator, { audio: true }, function (stream) {
            resolve(stream);
        }, function (err) {
            reject(err);
        });
    });
}

function beginOggVorbisEncoding(stream, quality, onStarted) {
    var bufferSize = 4 * 1024;

    var audioContext = new AudioContext();
    var audioSourceNode = audioContext.createMediaStreamSource(stream);
    var scriptProcessorNode = audioContext.createScriptProcessor(bufferSize);

    var channels = 2;
    var sampleRate = audioContext.sampleRate;

    var options = {
        workerURL: APP_URL + 'Resources/operador/recorder-plugin/lib/libvorbis.oggvbr.asyncencoder.worker.js',
        moduleURL: 'libvorbis.asmjs.min.js',
        encoderOptions: {
            channels: channels,
            sampleRate: sampleRate,
            quality: quality
        }
    };

    var chunks = [];
    var onFinishedCallback = null;

    function onData(data) {
        chunks.push(data);
    }

    function onFinished() {
        var blob = new Blob(chunks, { type: 'audio/ogg' });
        onFinishedCallback(blob);
    }

    libvorbis.OggVbrAsyncEncoder.create(options, onData, onFinished).then(function (encoder) {
        scriptProcessorNode.onaudioprocess = function (ev) {
            var inputBuffer = ev.inputBuffer;
            var samples = inputBuffer.length;

            var ch0 = inputBuffer.getChannelData(0);
            var ch1 = inputBuffer.getChannelData(1);

            // script processor reuses buffers; we need to make copies
            ch0 = new Float32Array(ch0);
            ch1 = new Float32Array(ch1);

            var channelData = [ch0, ch1];

            encoder.encode(channelData);
        };

        audioSourceNode.connect(scriptProcessorNode);
        scriptProcessorNode.connect(audioContext.destination);

        function stopper(onFinished) {
            onFinishedCallback = onFinished;

            audioSourceNode.disconnect(scriptProcessorNode);
            scriptProcessorNode.disconnect(audioContext.destination);

            audioContext.close();

            encoder.finish();
        }

        onStarted(encoder, stopper);
    });
}

window.addEventListener('load', function () {
    var record = document.querySelector('#record');

    record.addEventListener('click', function () {
        IniciarGrabacion();
    });
});

function IniciarGrabacion() {
    getAudioStream().then(function (stream) {
        beginOggVorbisEncoding(stream, 0.1, GrabacionIniciada);
    });
}

function GrabacionIniciada(encoder, stopper) {
    var stop = document.querySelector('#stop');

    stop.addEventListener('click', function onStopClicked() {
        stop.removeEventListener('click', onStopClicked);

        stopper(GrabacionFinalizada);
    });
}

function GrabacionFinalizada(blob) {
    uploadBlob(APP_URL + 'filavirtual/atenciones/' + Id + '/audio/send', blob);
}
