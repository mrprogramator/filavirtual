/// <reference path="lib/libvorbis.oggvbr.asyncencoder.worker.min.js" />
/// <reference path="lib/libvorbis.oggvbr.asyncencoder.worker.min.js" />
'use strict';

function uploadBlob(url, blob) {
  return new Promise(function (resolve, reject) {
    var xhr = new XMLHttpRequest();
    xhr.open('POST', url, true);
    console.log('sending audio to ', url);
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
  var getUserMedia  =  navigator.getUserMedia
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
      workerURL: 'lib/libvorbis.oggvbr.asyncencoder.worker.min.js',
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
  var stop = document.querySelector('#stop');
  console.log('loading audio and stop:' , stop);
  var output = document.querySelector('#output');
  
  var template = document.querySelector('#recording-template');
  
  var capture = null;
  
  record.addEventListener('click', function () {
    getAudioStream().then(function (stream) {
        console.log('beginning enconding XDDDDDD');

      beginOggVorbisEncoding(stream, 0.5, function onStarted(encoder, stopper) {
          console.log('beginning enconding LALALA XDDDDDD');
          console.log('stop on event', stop);
          stop.addEventListener('click', function onStopClicked() {

          stop.removeEventListener('click', onStopClicked);
          
          stopper(function (blob) {

            uploadBlob('~/Atencion/ReceiveRecord/?id=' + Id, blob);
            var url = URL.createObjectURL(blob);
            console.log('blob',blob,'url',url);
            
          });
          stream.stop();
        });
      });
    });
  });
  
});