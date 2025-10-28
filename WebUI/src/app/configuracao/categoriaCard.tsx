'use client'

import { useEffect, useState } from "react"
import { criarCategoria, deleteCategoria, editarCategoria, fetchCategorias } from "@/app/services/categoriaService"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardFooter, CardTitle } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Pencil, Trash, Check, ChevronRight, ChevronLeft } from 'lucide-react'

export default function CategoriaCard() {
    const [categorias, setCategorias] = useState<{ id: number; nome: string }[]>([])
    const [novaCategoria, setNovaCategoria] = useState("")
    const [editandoId, setEditandoId] = useState<number | null>(null); //guarda o ID da categoria que está sendo editada
    const [nomeEditado, setNomeEditado] = useState("");

    const [pageAtual, setPageAtual] = useState(1);
    const porPagina = 8

    useEffect(() => {
        carregarCategorias();
    }, []);

    async function carregarCategorias() {
        try{
            const data = await fetchCategorias();
            setCategorias(data);
        } catch(erro){
            console.error("Erro ao carregar categorias:", erro);
        }
    } 

    async function handleCriar(){
        if (!novaCategoria.trim()) return;

        try {
            await criarCategoria(novaCategoria);
            setNovaCategoria("");
            await carregarCategorias();
        } catch (erro) {
            console.error(erro);
        }
    }

    async function handleDelete(id: number){
        try{
            await deleteCategoria(id);
            await carregarCategorias();
        } catch (erro) {
        console.error(erro);
        }
    }

    async function handleEditar(id: number){
        try {
            await editarCategoria(id, nomeEditado);
            setEditandoId(null);
            setNomeEditado("");
            await carregarCategorias();
        } catch (erro) {
            console.error(erro);
        }
    };

    const totalPaginas = Math.ceil(categorias.length / porPagina)
    const inicio = (pageAtual - 1) * porPagina
    const fim = inicio + porPagina
    const categoriasPaginada = categorias.slice(inicio, fim)

    useEffect(() => {
        if(totalPaginas === 0){
            setPageAtual(1);
        }else if (pageAtual > totalPaginas){
            setPageAtual(totalPaginas);
        }
        
    },[categorias.length, pageAtual]);


    return(
        <div className=" w-[95%] min-h-96">
            <Card className=" w-[50%] min-h-96  p-4 mx-auto">
                <CardTitle className="text-3xl justify-center flex">CATEGORIAS</CardTitle>
                <CardContent>
                    <ul>
                        {categoriasPaginada.map((categoria) => (
                        <li key={categoria.id}>
                            <div className="flex">
                                <Button 
                                    variant="ghost"
                                    onClick={() => {
                                        setEditandoId(categoria.id); //Aqui define qeum está sendo editado
                                        setNomeEditado(categoria.nome) //Preenche o input de edição com o nome atual da categoria
                                    }}
                                >
                                    <Pencil className=" text-blue-500" />
                                </Button>
                                <Button 
                                    variant="ghost"
                                    onClick={() => handleDelete(categoria.id)}
                                >
                                    <Trash className="text-red-500" />
                                </Button>
                                {editandoId === categoria.id ? (
                                <>
                                    <Input
                                    value={nomeEditado}
                                    onChange={(e) => setNomeEditado(e.target.value)}
                                    className="max-w-xs"
                                    />
                                    <Button
                                    variant="ghost"
                                    onClick={() => handleEditar(categoria.id)}
                                    >
                                    <Check className="size-[19px] text-green-600" />
                                    </Button>
                                </>
                                ) : (
                                <p className="p-1 mt-1">{categoria.nome}</p>
                                )}
                            </div>
                        </li>
                        ))}
                    </ul>
                </CardContent>
                <div className="flex justify-between mt-2">
                    <Button
                    variant="ghost"
                    disabled= {pageAtual === 1}
                    onClick={() => setPageAtual(p => p - 1)}
                    >
                        <ChevronLeft className="size-[22px] text-[#12698A]"/>
                    </Button>
                    <p className="mt-1.5">
                        Página {pageAtual} de {totalPaginas === 0 ? 1 : totalPaginas}
                    </p>
                    <Button
                    variant="ghost"
                    disabled={pageAtual === totalPaginas || totalPaginas === 0}
                    onClick={() => setPageAtual(p => p + 1)}
                    >
                        <ChevronRight className="size-[22px] text-[#12698A]"/>
                    </Button>
                </div>
                <CardFooter className="gap-2 mt-auto">  
                <Input
                    placeholder="Digite sua Categoria"
                    value={novaCategoria}
                    onChange={(e) => setNovaCategoria(e.target.value)}
                />
                <Button
                    variant="secondary"
                    className="bg-[#12698A] font-bold"
                    onClick={() => handleCriar()}
                >
                    Enviar
                </Button>
                </CardFooter>
            </Card>
        </div>
    )
}