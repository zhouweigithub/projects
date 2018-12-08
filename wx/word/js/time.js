var playSeconds = 0;
var timeToken;

function StartSeconds() {
  timeToken = setInterval(function() {
    if (playSeconds < 3599)
      playSeconds += 1;
    else
      clearInterval(timeToken);
  }, 1000);
}

function StopSeconds() {
  clearInterval(timeToken);
}

function GetPlaySeconds() {
  return playSeconds;
}

function GetPlaySecondsFormatString() {
  return formatTime(playSeconds);
}

function ClearPlaySeconds() {
  playSeconds = 0;
}

function formatTime(time) {
  var minute = parseInt(time / 60);
  var second = time % 60;
  return (minute < 10 ? "0" : "") + minute + ":" + (second < 10 ? "0" : "") + second;
}

module.exports = {
  StartSeconds: StartSeconds,
  StopSeconds: StopSeconds,
  GetPlaySeconds: GetPlaySeconds,
  ClearPlaySeconds: ClearPlaySeconds,
  GetPlaySecondsFormatString: GetPlaySecondsFormatString,
  formatTime: formatTime,
}