# Moonset

### SRP Project

We were one of the groups who decided before this course began to work with URP when developing our game. Unfortunatly we could not work every shader and effect into our project but we did compile them in a separate project.

[SRP Project Repo](https://github.com/ShockWaveGamer/Graphics-SRP-Final.git)

### Controls
- WASD / Movement
- Space / Jump
- Left Click / Grapple 
- Right Click / Slow Mo
- Ctrl / Stomp
- Shift / Sprint

### Debug Shortcuts
- Tab + T / Toggle Textures
- Tab + F / Toggle Color Blind Mode

## Objective
Your goal is to manuver your way to the end of the level guided by yellow waypoints, swinging from yellow dominant objects in the scene, as fast as possible.

# Explanations (Feature - Credits)

## Post Processing & Colour Correction
### Colour Correction - Constantine / Daniel
  We used our palette shader to achieve the same effects of a LUT, but in a more restrictive way.  We start by globally grading the image to ACES and we use a custom Blit Material Feature to apply our changes to the image. Our game has two of these filters, the default one and one for colorblind players.
![image](https://user-images.githubusercontent.com/88565667/229962791-4b559db9-8590-4ee1-910f-1010917e3e97.png)
![image](https://user-images.githubusercontent.com/88565667/229962692-88b88f1b-0576-4352-9abe-404da7a52ac6.png)
![image](https://user-images.githubusercontent.com/88565667/229962973-3fb4ef7e-b843-46b3-b0bc-5e3756b63f18.png)

## Visual Effects
### Particles - Constantine
  A custom particle texture and shader were created. We were able to overwrite our game's lack of transparency by locally rendering the transparent effect. The particle effect is used to guide players towards checkpoints, so it has eye catching effects such as a gradual change in scale and rotation.
![image](https://user-images.githubusercontent.com/88565667/229964297-c13a127c-a4b8-4190-8347-4cb7690a3a4b.png)
![image](https://user-images.githubusercontent.com/88565667/229964348-7a551318-230a-4173-9fe6-38a4d5aa33c7.png)

### Decals - Daniel
  I used the base of the decal shader taught in the lectures. However it has been modified into a vertex / fragment shader compatible with HLSL. I've also added the ability to translate, rotate, and scale. By manipulating the uv's of both the main texture and decal texture. 
![image](https://user-images.githubusercontent.com/88565667/229958942-171f178e-bf5b-43b8-a7c9-23d3a2ac3a35.png)
![image](https://user-images.githubusercontent.com/88565667/229958668-4ce48802-bae3-40a0-9370-ad92c638af8f.png)

## Additional Effects
### ToonShading - Constantine
This effect is also done within our Palette shader. The effect that is analogous to colour ramping is caused by removing any dithering and anti-aliasing from the edges caused by the colour limitations. It creates a very flat look. This is accomplished through the color correction.

### Outline - Constantine

### Rim Lighting - Daniel
  Using the rim lighting presented in the lectures as a base. I modified it to be compatable with HLSL. By comparing the view direction and the vertex normals, the dot product we cas set the colors of the object.
![image](https://user-images.githubusercontent.com/88565667/229961296-706bb99d-ab50-4625-9ef8-f91c411377e3.png)
![image](https://user-images.githubusercontent.com/88565667/229961411-1fdf4b43-1d67-4e9a-b7ef-70079a61a50d.png)

### Vertex Extrusion - Daniel 
  Using the shaders in class as a base, it has been modified to be compatible with URP. We can manipulate the vertex positions by moving the vertex along the normal direction by an amount set in the inspector.
![image](https://user-images.githubusercontent.com/88565667/229961717-6e44694f-686f-4258-beeb-149a863ca3c5.png)
![image](https://user-images.githubusercontent.com/88565667/229961763-2620e06e-2de4-4850-aa2b-5d1a19868932.png)

## Additional Post Processing Effects
### Pixelation - Constantine

### Fog - Daniel
  By reffrenceing the depthmap of the scene camera, we can mutiply the white fade by a fog color. We can then add this ontop of the camera picture to create a fog effect.
![image](https://user-images.githubusercontent.com/88565667/229961878-b6950c1f-b892-4c25-8366-e7a277e660a2.png)
![image](https://user-images.githubusercontent.com/88565667/229961822-96480c43-796b-4e47-a4ef-3704a91d53d9.png)
