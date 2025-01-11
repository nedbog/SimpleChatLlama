# Pull the Ollama container
docker run --gpus all -d -v ollama_data:/root/.ollama -p 11434:11434 --name ollama ollama/ollama

# Pull the llama3 model
docker exec -it ollama ollama pull llama3
