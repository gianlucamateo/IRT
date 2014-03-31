clear all
close all
baseY = 0;
initialRayPosition = [0;0.5];
initialRayDirection = [1;-0.5];
n0 = 1;
stepSize = 0.001;
bigStepSize = 0.01;

v = -0:stepSize:1.1;
[x,y] = meshgrid(v);
n = @(x,y) n0+ x + y ;
z = n0 + x + y;
[px,py]=gradient(z,stepSize,stepSize);
initialRayDirection = initialRayDirection./norm(initialRayDirection);


r = initialRayPosition;
sum = 0;
doubleSum = 0;
rayDir = initialRayDirection; 
hold on
dl2 = 0;
for dl1 = 0: bigStepSize: 1
    for dl2 = dl1 : stepSize : dl2+bigStepSize
        dirStep = (dl2-dl1) .* rayDir    
        xT = px(1+floor((r(1)+dirStep(1))/stepSize),1+floor((r(2)+dirStep(2))/stepSize));
        yT = py(1+floor((r(1)+dirStep(1))/stepSize),1+floor((r(2)+dirStep(2))/stepSize));
        grad = [xT;yT]
        dr = grad*stepSize;
        doubleSum = doubleSum + sum;
        sum = sum + dr
    end
doubleSum = doubleSum*stepSize
rayDir
rayDir = (rayDir + doubleSum)./norm(rayDir + doubleSum)
r = r + rayDir.*bigStepSize
axis equal;
plot(r(1),r(2),'+r')
end
hold off

