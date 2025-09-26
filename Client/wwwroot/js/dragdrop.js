window.dragDropHelper = {
    dragData: {},
    setData: function (event, key, value) {
        this.dragData[key] = value;
    },
    getData: function (event, key) {
        return this.dragData[key];
    }
}