var app = angular.module('app', []);

app.factory('Question', ['$http', function ($http) {

    return {
        Save: save,
        Get: get,
        GetOne: getOne
    };

    function save(entity) {
        return $http.post('/api/Question/Save', entity).then(complete).catch(failed);
    };

    function get() {
        return $http.get('/api/Question/Get').then(complete).catch(failed);
    };

    function getOne(id) {
        return $http.get('/api/Question/GetOne', id).then(complete).catch(failed);
    };

    function complete(response) {
        return response.data;
    }

    function failed(error) {
        console.log(error.statusText);
    }

}]);

app.controller('QuestionController', ['$scope', 'Question', function ($scope, Question) {

    $scope.entity = {};

    $scope.Save = function () {
        Question
            .Save($scope.entity)
            .then(function (data) {
                alert("OK");
                $scope.entity = {};
            });
    };

}]);