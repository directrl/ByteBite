#version 330

layout (location=0) in vec3 position;
layout (location=1) in vec2 texCoord;
layout (location=2) in vec3 normal;

out vec2 outTexCoord;
out vec3 outFragPos;
out vec3 outNormal;

uniform mat4 projection;
uniform mat4 view;
uniform mat4 model;

void main() {
	outTexCoord = texCoord;
	outFragPos = vec3(model * vec4(position, 1.0));
	outNormal = mat3(transpose(inverse(model))) * normal;
	
	gl_Position = projection * view * vec4(outFragPos, 1.0);
}