'use client'

import { useEffect, useState } from "react"
import { fetchPessoas, criarPessoa, deletarPessoa, editarPessoa } from "@/app/services/pessoaService"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardFooter, CardTitle } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Pencil, Trash, Check, ChevronRight, ChevronLeft } from 'lucide-react'

export default function PessoasCard() {
    const [pessoas, setPessoas] = useState<{ id: number; nome: string }[]>([])
    const [novaPessoa, setNovaPessoa] = useState("")
    const [editandoId, setEditandoId] = useState<number | null>(null)
    const [nomeEditado, setNomeEditado] = useState<string>("")

    const [paginaAtual, setPaginaAtual] = useState(1);
    const porPagina = 8

    useEffect(() => {
        carregarPessoas();
    }, [])

    async function carregarPessoas() {
        try {
            const data = await fetchPessoas();
            setPessoas(data);
        } catch (erro) {
            console.error("Erro ao carregar pessoas:", erro);
        }
    }

    async function handleCriar() {
        if (!novaPessoa.trim()) return;
        try {
            await criarPessoa(novaPessoa);
            setNovaPessoa("");
            await carregarPessoas();
        } catch (erro) {
            console.error(erro);
        }
    }

    async function handleDelete(id: number) {
        try {
            await deletarPessoa(id);
            await carregarPessoas();
        } catch (erro) {
            console.error(erro);
        }
    }

    async function handleEditar(id: number, name: string) {
        try {
            await editarPessoa(id, name);
            setEditandoId(null);
            setNomeEditado("");
            await carregarPessoas();
        } catch (erro) {
            console.error(erro);
        }
    }

    const totalPaginas = Math.ceil(pessoas.length / porPagina)
    const inicio = (paginaAtual - 1) * porPagina
    const fim = inicio + porPagina
    const pessoasPaginadas = pessoas.slice(inicio, fim)

    useEffect(() => {
        if(totalPaginas === 0){
            setPaginaAtual(1);
        }else if (paginaAtual > totalPaginas){
            setPaginaAtual(totalPaginas);
        }
    },[pessoas.length, paginaAtual]);

    return (
        <div className="w-[95%] min-h-96">
            <Card className="w-[50%] min-h-96  p-4 mx-auto">
                <CardTitle className="text-3xl justify-center flex">PESSOAS</CardTitle>
                <CardContent>
                    <ul>
                        {pessoasPaginadas.map((pessoa) => (
                            <li key={pessoa.id}>
                                <div className="flex">
                                    <Button 
                                    variant="ghost" 
                                    onClick={() => {
                                        setEditandoId(pessoa.id)
                                        setNomeEditado(pessoa.nome)
                                    }}>
                                        <Pencil className="text-blue-500" />
                                    </Button>
                                    <Button variant="ghost" onClick={() => handleDelete(pessoa.id)}>
                                        <Trash className="text-red-500" />
                                    </Button>
                                    {pessoa.id === editandoId ? (
                                        <>
                                            <Input
                                                value={nomeEditado}
                                                onChange={(e) => setNomeEditado(e.target.value)}
                                                className="max-w-xs"
                                            />
                                            <Button
                                                variant="ghost"
                                                onClick={() => handleEditar(pessoa.id, nomeEditado)}
                                            >
                                                <Check className="text-green-600 size-[19px]" />
                                            </Button>
                                        </>
                                    ) : (
                                        <p className="p-1 mt-1">{pessoa.nome}</p>
                                    )}
                                </div>
                            </li>
                        ))}
                    </ul>
                </CardContent>

                {/* Paginação */}
                <div className="flex justify-between px-4 mt-2">
                    <Button
                        variant="ghost"
                        disabled={paginaAtual === 1}
                        onClick={() => setPaginaAtual(p => p - 1)}
                    >
                        <ChevronLeft className="size-[22px] text-[#12698A]"/>
                    </Button> 
                    <p className="mt-1.5">
                        Página {paginaAtual} de {totalPaginas === 0 ? 1 : totalPaginas}
                    </p>
                    <Button
                        variant="ghost"
                        disabled={paginaAtual === totalPaginas || totalPaginas === 0}
                        onClick={() => setPaginaAtual(p => p + 1)}
                    >
                        <ChevronRight className="size-[22px] text-[#12698A]"/>
                    </Button>
                </div>

                <CardFooter className="gap-2 mt-auto">
                    <Input
                        placeholder="Digite o nome da Pessoa"
                        value={novaPessoa}
                        onChange={(e) => setNovaPessoa(e.target.value)}
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
