'use client';

import React from 'react';
import { useRouter } from 'next/navigation';
import s from './knop.module.css';

type BiedKnopProps = {
    label: string;
    to: string;
};

const BiedKnop: React.FC<BiedKnopProps> = ({ label, to }) => {
    const router = useRouter();

    const handleClick = () => {
        router.push(to);
    };

    return (
        <button className={s.button} onClick={handleClick}>
            {label}
        </button>
    );
};

export default BiedKnop;
