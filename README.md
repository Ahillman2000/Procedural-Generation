# Procedural Cityscapes Using WFC

# Project Description
This project procedurally generates a 3d cityscape from a given set of tiles and connection rules. Taking inspiration from the [WaveFunctionCollapse](https://github.com/mxgmn/WaveFunctionCollapse) algorithm by Maxim Gumin, the created system generates a grid of a given size which can then be populated with tile values, either manually by the user or procedurally collapsed by the function.

![WFC city](https://user-images.githubusercontent.com/55785328/179972456-5a03cf36-38c9-4a22-b93a-999d205d3b13.jpg)

# Installing / Using the Project
The project is built in Unity 2021.1.21. 

To use project UI is provided to alter apsects such as grid size or solver delay. The generate button creates a new grid of the provided size and can be used to reset the grid if needed. To solve the grid users can select any of the tiles in an uncollapsed cell to set that cells value, the solve button can be used to automatically solve any remaining empty cells.

# Documentation
The written portion of this dissertation project can be found here: [Procedural Generation of Virtual Cityscapes Using WaveFunctionCollapse Algorithm.pdf](https://github.com/Ahillman2000/Procedural-Generation/files/10359346/Procedural.Generation.of.Virtual.Cityscapes.Using.WaveFunctionCollapse.Algorithm.pdf)
# References
- Donald, M., (2020) Superpositions, Sudoku, the Wave Function Collapse algorithm. Available from: https://www.youtube.com/watch?v=2SuvO4Gi7uY
- Gumin, M. (2016) Wave Function Collapse Algorithm. Available from: https://github.com/mxgmn/WaveFunctionCollapse
- Marian42 (2019) Infinite procedurally generated city with the Wave Function Collapse algorithm. Available from: https://marian42.de/article/wfc/
- Stålberg, O. https://oskarstalberg.com/game/wave/wave.html

