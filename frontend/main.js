window.addEventListener('DOMContentLoaded', (event) => {
    getVisitCount();
});

const functionApi = '';

const getVisitCount = () => {
    let count = 30;
    fetch(functionApi).then(respoonse => {
        return respoonse.json()
    }).then(response => {
        console.log("Webste called functionApi");
        count = response.count;
        document.getElementById("counter").innerText = count;
    }).catch(function (error) {
        console.log(error);
    });
    return count;
}
