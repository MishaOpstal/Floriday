'use client';

import React from 'react';
import s from './dashboardPanel.module.css';

type DashboardPanelProps = {
    title: string;
    info: string;
    imageSrc?: string;
    children?: React.ReactNode;
};

const DashboardPanel: React.FC<DashboardPanelProps> = ({ title, info, imageSrc, children }) => {

    return (
        <div className={s.panel}>
            {imageSrc && <img src={imageSrc} alt="Afbeelding" className={s.image} />}

            <div className={s.middle}>
                <div className={s.title}>{title}</div>
                <div className={s.content}>{info}</div>

                {children && <div className={s.actions}>{children}</div>}
            </div>
        </div>
    );
};

export default DashboardPanel;
