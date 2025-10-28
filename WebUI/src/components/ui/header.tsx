"use client"

import { Button } from "@/components/ui/button";
import Link from "next/link"; 
import { usePathname } from "next/navigation";

export function Header(){

    const pathname = usePathname();
    const isHome = pathname === "/";

    if(isHome) return null;

    const links = [
        {href: "/", label: "Home"},
        {href: "/perfil", label: "Movimentos Fixos"},
        {href: "/extratos", label: "Extratos"},
        {href: "/configuracao", label: "Configuração"},
    ]

    return(
        <div data-aos="fade-down" className="bg-[#12698A] h-10 flex items-center justify-center p-2 gap-1 shadow-md z-10 relative">
            {links.map((link) => {
                const isActive = pathname === link.href;

                return(
                    <Link key={link.href} href={link.href}>
                        <Button 
                        variant="secondary" 
                        className={`bg-[#12698A] text-white font-bold ${isActive ? "bg-[#0f5975]" : ""}`}>
                            {link.label}
                        </Button>
                    </Link>
                )
            })}
        </div>
    )
}