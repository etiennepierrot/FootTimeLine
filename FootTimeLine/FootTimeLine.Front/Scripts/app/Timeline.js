function Game(homeTeam, awayTeam, league, hashTag) {
    var self = this;
    self.homeTeam = ko.observable(homeTeam);
    self.awayTeam = ko.observable(awayTeam);
    self.league = ko.observable(league);
    self.hashTag = ko.observable(hashTag);
}

function TimelineViewModel(homeTeam, awayTeam, league, hashtag) {
    var self = this;
    self.game = ko.observable(new Game(homeTeam, awayTeam, league, hashtag));
    self.getTimeline = function() {
        var gameModelPost = ko.mapping.toJS(self.game);
        ko.utils.postJson("/Home/GetTimeline", { gameModelPost : gameModelPost});
    }
}