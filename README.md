# PGC-Mesh-Animation


PGC_Mesh Generation

  -A mesh is procedurally genereated with Perlin noise throughout a vertice array
  
  -Individual triangles are created for vertice points based on user settings inside PGC_Controller for mesh size
 # 
PGC_Controller

  -X Size: Width of mesh
  
  -Z Size: Length of mesh
  
  -Cube X, Y, Z: Starting coordinates for cube
  
  -Sphere X, Y, Z: Starting coordinates for sphere
  
  -Wavelength: Controls the wavelength of the wave. Default is set to 50 to show a longer wave
  
  -Amplitude: Controls height of the wave, default is set to 2
  
  -Speed: Controls how fast the wave animates, default is set to 5
#  
Mesh Animation

  -Each frame, the mesh vertices are updated with a sine function to create a wave motion
  
  -A more complex trig function can be used to simulate real-world water effects, but generally over a large space something akin to the sine fucntion in use suits most applications
  
  -This animation does not affect the Perlin noise seed on the mesh but rather animates the entire mesh after generation. This could be used in combination with a Perlin noise texture for even more success in creating water effects
  #
Cube

  -This cube was created to test the mesh's collider during animation as well as creating a platonic solid mesh cube with 8 connecting corners from 12 total triangles
  #
Sphere

  -This primitive sphere was used in conjuction with the cube to test the mesh's collider
  
  -The sphere collider is removed on generation and replaced with a mesh collider for better collision
