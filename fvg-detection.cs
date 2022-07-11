

public bool isFvgUp(int index) {
    return low[index] > high[index + 2];
}

public bool isFvgDown(int index) {
    return high[index] < low[index + 2];
}

public void plotFvg() {
    if (isFvgUp(0)) {
        
    } 
    
    if (isFvgDown(0)) {

    }
}