#version 330

struct Material {
	int type;
	int specularType;
	
	vec3 albedo;
	
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
	
	float metallic;
};

struct Light {
	Material material;
	vec3 position;
};

in vec2 outTexCoord;
in vec3 outFragPos;
in vec3 outNormal;

out vec4 fragColor;

uniform sampler2D texSampler;
uniform vec3 cameraPos;
uniform Material material;
uniform Light light;

void main() {
	if(material.type == 0) { // light
		fragColor = vec4(1.0);
	} else {
		// ambient
		vec3 ambient = light.material.ambient * material.ambient;

		// diffuse
		vec3 norm = normalize(outNormal);
		vec3 lightDir = normalize(light.position - outFragPos);
		float diff = max(dot(norm, lightDir), 0.0);
		vec3 diffuse = light.material.diffuse * (diff * material.diffuse);

		// specular
		vec3 viewDir = normalize(cameraPos - outFragPos);
		vec3 reflectDir = reflect(-lightDir, norm);
		float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.metallic);
		vec3 specular = light.material.specular * (spec * material.specular);

		vec3 result = ambient + diffuse + specular;
		fragColor = texture(texSampler, outTexCoord) * vec4(material.albedo, 1.0) * vec4(result, 1.0);
	}
}