const BASE_URL = 'http://localhost:5189/api/TiposMovimentos';

export async function fetchMovimentos(): Promise<TipoMovimento[]> {
    const response = await fetch(`${BASE_URL}/Display-Movimentos`, {
        cache: "no-store"
    });
    if(!response.ok){
        throw new Error("Falha ao buscar movimentos.")
    }
    return response.json();
}