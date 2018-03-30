function Game(homeTeam, awayTeam, league, hashTag) {
    var self = this;
    self.homeTeam = ko.observable(homeTeam);
    self.awayTeam = ko.observable(awayTeam);
    self.league = ko.observable(league);
    self.hashTag = ko.observable(hashTag);
}

function TimelineViewModel() {
    var self = this;
    self.game = ko.observable(new Game("Marseille", "Lyon", "Ligue 1", "#OMOL"));
    self.getTimeline = function() {
        var gameModelPost = ko.mapping.toJS(self.game);
        ko.utils.postJson("/Home/GetTimeline", { gameModelPost : gameModelPost});
    }
}